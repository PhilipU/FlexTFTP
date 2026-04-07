using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace FlexTFTP
{
    /// <summary>
    /// Checks FPGA compatibility after flashing S19 files by querying the device's REST API
    /// </summary>
    public static class FpgaCompatibilityChecker
    {
        private const int MaxWaitTimeSeconds = 150; // 2.5 minutes
        private const int PingIntervalSeconds = 5;
        private const int RestApiDelaySeconds = 1;
        private const int HttpTimeoutSeconds = 30;
        private const bool EnableDebugOutput = false; // Set to false to disable debug output

        /// <summary>
        /// Checks FPGA compatibility by comparing required FPGA images from /api/project with loaded images from /api/fpga
        /// </summary>
        /// <param name="ipAddress">IP address of the device</param>
        /// <param name="outputBox">OutputBox for logging messages</param>
        /// <param name="s19FilePath">Path to the S19 file that was transferred (optional)</param>
        /// <param name="form">Main form for triggering transfers</param>
        public static void CheckCompatibilityAsync(string ipAddress, OutputBox? outputBox, string? s19FilePath = null, FlexTftpForm? form = null)
        {
            if (EnableDebugOutput)
                outputBox?.AddLine("[DEBUG] FPGA Check: Method entered", Color.Gray, true);
            
            if (string.IsNullOrEmpty(ipAddress) || outputBox == null)
            {
                if (EnableDebugOutput)
                    outputBox?.AddLine("[DEBUG] FPGA Check: Invalid parameters, exiting", Color.Gray, true);
                return;
            }

            Thread.Sleep(RestApiDelaySeconds * 1000);

            try
            {
                // Check if device is already online
                bool isAlreadyOnline = false;
                try
                {
                    using (var ping = new Ping())
                    {
                        var reply = ping.Send(ipAddress, 1000);
                        isAlreadyOnline = (reply?.Status == IPStatus.Success);
                    }
                }
                catch
                {
                    isAlreadyOnline = false;
                }

                // Only show "waiting" message if device is not yet online
                if (!isAlreadyOnline)
                {
                    outputBox.AddLine("Waiting for device to restart...", Color.Gray, true);

                    // Wait for device to come back online
                    if (EnableDebugOutput)
                        outputBox.AddLine("[DEBUG] FPGA Check: Calling WaitForDeviceOnline", Color.Gray, true);
                    
                    if (!WaitForDeviceOnline(ipAddress, outputBox))
                    {
                        outputBox.AddLine("Device did not respond, skipping FPGA check", Color.Gray, true);
                        if (EnableDebugOutput)
                            outputBox.AddLine("[DEBUG] FPGA Check: Device timeout, exiting", Color.Gray, true);
                        return;
                    }
                }
                else if (EnableDebugOutput)
                {
                    outputBox.AddLine("[DEBUG] Device already online, skipping restart wait", Color.Gray, true);
                }

                // Additional delay for REST API to be ready
                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] FPGA Check: Waiting {RestApiDelaySeconds}s for REST API", Color.Gray, true);
                Thread.Sleep(RestApiDelaySeconds * 1000);

                // Get required FPGA images
                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] FPGA Check: Calling GetRequiredFpgas({ipAddress})", Color.Gray, true);
                
                var requiredFpgas = GetRequiredFpgas(ipAddress, outputBox);
                if (requiredFpgas == null || requiredFpgas.Count == 0)
                {
                    outputBox.AddLine($"Failed to check for required FPGA images", Color.Gray, true);
                    return;
                }

                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] FPGA Check: Found {requiredFpgas.Count} required FPGA(s)", Color.Gray, true);

                // Get loaded FPGA images
                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] FPGA Check: Calling GetLoadedFpgas({ipAddress})", Color.Gray, true);
                
                var loadedFpgas = GetLoadedFpgas(ipAddress, outputBox);
                if (loadedFpgas == null || loadedFpgas.Count == 0)
                {
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] FPGA Check: No loaded FPGAs found (null={loadedFpgas == null}, count={loadedFpgas?.Count ?? 0})", Color.Gray, true);
                    return;
                }

                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] FPGA Check: Found {loadedFpgas.Count} loaded FPGA(s)", Color.Gray, true);

                // Find required FPGAs that are not compatible
                var missingUpdates = new List<FpgaRequirement>();
                foreach (var required in requiredFpgas)
                {
                    var loaded = loadedFpgas.FirstOrDefault(l => l.Id == required.Id);
                    
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] Checking required ID={required.Id}: loaded={loaded != null}, compat={loaded?.ProjectCompatibility}", Color.Gray, true);
                    
                    // Check if FPGA needs update
                    bool needsUpdate = false;
                    
                    if (loaded == null)
                    {
                        if (EnableDebugOutput)
                            outputBox.AddLine($"[DEBUG] FPGA ID={required.Id} not loaded", Color.Gray, true);
                        needsUpdate = true;
                    }
                    else if (required.FpgaImage?.Type == "0x0000")
                    {
                        // Type 0x0000 means "any FPGA image allowed" - wildcard
                        if (EnableDebugOutput)
                            outputBox.AddLine($"[DEBUG] FPGA ID={required.Id} wildcard type (0x0000), any image accepted", Color.Gray, true);
                        needsUpdate = false;
                    }
                    else if (loaded.FpgaImage?.Type != required.FpgaImage?.Type)
                    {
                        if (EnableDebugOutput)
                            outputBox.AddLine($"[DEBUG] FPGA ID={required.Id} type mismatch: required={required.FpgaImage?.Type}, loaded={loaded.FpgaImage?.Type}", Color.Gray, true);
                        needsUpdate = true;
                    }
                    else if (required.FpgaImage?.Version != "0.0.0.0")
                    {
                        // Version 0.0.0.0 means "any version allowed"
                        // Otherwise check if loaded version is older than required version
                        // Newer versions are always acceptable and should not trigger an update
                        int versionComparison = CompareVersions(loaded.FpgaImage?.Version, required.FpgaImage?.Version);
                        if (versionComparison < 0)
                        {
                            // Loaded version is older than required - needs update
                            if (EnableDebugOutput)
                                outputBox.AddLine($"[DEBUG] FPGA ID={required.Id} version too old: required={required.FpgaImage?.Version}, loaded={loaded.FpgaImage?.Version}", Color.Gray, true);
                            needsUpdate = true;
                        }
                        else if (EnableDebugOutput && versionComparison > 0)
                        {
                            // Loaded version is newer - this is acceptable
                            outputBox.AddLine($"[DEBUG] FPGA ID={required.Id} has newer version: required={required.FpgaImage?.Version}, loaded={loaded.FpgaImage?.Version} (OK)", Color.Gray, true);
                        }
                    }
                    
                    if (needsUpdate)
                    {
                        if (EnableDebugOutput)
                            outputBox.AddLine($"[DEBUG] FPGA ID={required.Id} needs update", Color.Gray, true);
                        missingUpdates.Add(required);
                    }
                    else
                    {
                        if (EnableDebugOutput)
                            outputBox.AddLine($"[DEBUG] FPGA ID={required.Id} is compatible", Color.Gray, true);
                    }
                }

                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] FPGA Check: Found {missingUpdates.Count} FPGA(s) requiring update", Color.Gray, true);

                if (missingUpdates.Count > 0)
                {
                    // Build warning message with required FPGA details
                    var fpgaDetails = string.Join(", ", missingUpdates.Select(f => 
                        $"{f.FpgaImage?.Type} v{f.FpgaImage?.Version}"));
                    
                    var warningMessage = $"FPGA Update required: {fpgaDetails}";
                    outputBox.AddLine(warningMessage, Color.DarkRed, true);

                    // Trigger auto-update if enabled
                    TriggerAutoUpdate(missingUpdates, ipAddress, outputBox, s19FilePath, form);
                }
                else
                {
                    // All compatible
                    outputBox.AddLine("FPGA compatibility check: OK", Color.Green, true);
                }
            }
            catch (Exception ex)
            {
                if (EnableDebugOutput)
                {
                    outputBox?.AddLine($"[DEBUG] FPGA Check: Exception caught: {ex.Message}", Color.Red, true);
                    outputBox?.AddLine($"[DEBUG] FPGA Check: Stack trace: {ex.StackTrace}", Color.Gray, true);
                }
                // Silently ignore all errors as requested
            }
        }

        /// <summary>
        /// Checks FPGA compatibility for CLI mode (console output)
        /// </summary>
        /// <param name="ipAddress">IP address of the device</param>
        /// <param name="s19FilePath">Path to the S19 file that was transferred (optional)</param>
        /// <returns>Path to FPGA container file if update is needed, null otherwise</returns>
        public static async Task<string?> CheckCompatibilityCliAsync(string ipAddress, string? s19FilePath = null)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return null;
            }

            try
            {
                // Check if device is already online
                bool isAlreadyOnline = false;
                try
                {
                    using (var ping = new Ping())
                    {
                        var reply = ping.Send(ipAddress, 1000);
                        isAlreadyOnline = (reply?.Status == IPStatus.Success);
                    }
                }
                catch
                {
                    isAlreadyOnline = false;
                }

                // Only show "waiting" message if device is not yet online
                if (!isAlreadyOnline)
                {
                    Utils.WriteLine("(i) Waiting for device to restart...");
                    
                    // Wait for device to come back online
                    if (!WaitForDeviceOnlineCli(ipAddress))
                    {
                        Utils.WriteLine("(x) Device did not respond, skipping FPGA check");
                        return null;
                    }
                }
                else if (EnableDebugOutput)
                {
                    Utils.WriteLine("[DEBUG] Device already online, skipping restart wait");
                }

                // Additional delay for REST API to be ready
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] Waiting {RestApiDelaySeconds}s for REST API");
                Thread.Sleep(RestApiDelaySeconds * 1000);

                // Get required FPGA images
                if (EnableDebugOutput)
                    Utils.WriteLine("[DEBUG] Calling GetRequiredFpgasCliAsync...");
                var requiredFpgas = await GetRequiredFpgasCliAsync(ipAddress);
                
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] Required FPGAs: {requiredFpgas?.Count ?? 0}");
                
                if (requiredFpgas == null || requiredFpgas.Count == 0)
                {
                    if (EnableDebugOutput)
                        Utils.WriteLine("[DEBUG] No required FPGAs found, exiting");
                    return null;
                }

                if (EnableDebugOutput)
                {
                    foreach (var req in requiredFpgas)
                    {
                        Utils.WriteLine($"[DEBUG] Required: ID={req.Id}, Type={req.FpgaImage?.Type}, Version={req.FpgaImage?.Version}");
                    }
                }

                // Get loaded FPGA images
                if (EnableDebugOutput)
                    Utils.WriteLine("[DEBUG] Calling GetLoadedFpgasCliAsync...");
                var loadedFpgas = await GetLoadedFpgasCliAsync(ipAddress);
                
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] Loaded FPGAs: {loadedFpgas?.Count ?? 0}");
                
                if (loadedFpgas == null || loadedFpgas.Count == 0)
                {
                    if (EnableDebugOutput)
                        Utils.WriteLine("[DEBUG] No loaded FPGAs found, exiting");
                    return null;
                }

                if (EnableDebugOutput)
                {
                    foreach (var loaded in loadedFpgas)
                    {
                        Utils.WriteLine($"[DEBUG] Loaded: ID={loaded.Id}, Type={loaded.FpgaImage?.Type}, Version={loaded.FpgaImage?.Version}, Compatibility={loaded.ProjectCompatibility}");
                    }
                }

                // Find required FPGAs that are not compatible
                var missingUpdates = new List<FpgaRequirement>();
                foreach (var required in requiredFpgas)
                {
                    var loaded = loadedFpgas.FirstOrDefault(l => l.Id == required.Id);
                    
                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] Checking required ID={required.Id}: loaded={loaded != null}, compat={loaded?.ProjectCompatibility}");
                    
                    // Check if FPGA needs update
                    bool needsUpdate = false;
                    
                    if (loaded == null)
                    {
                        if (EnableDebugOutput)
                            Utils.WriteLine($"[DEBUG] FPGA ID={required.Id} not loaded");
                        needsUpdate = true;
                    }
                    else if (required.FpgaImage?.Type == "0x0000")
                    {
                        // Type 0x0000 means "any FPGA image allowed" - wildcard
                        if (EnableDebugOutput)
                            Utils.WriteLine($"[DEBUG] FPGA ID={required.Id} wildcard type (0x0000), any image accepted");
                        needsUpdate = false;
                    }
                    else if (loaded.FpgaImage?.Type != required.FpgaImage?.Type)
                    {
                        if (EnableDebugOutput)
                            Utils.WriteLine($"[DEBUG] FPGA ID={required.Id} type mismatch: required={required.FpgaImage?.Type}, loaded={loaded.FpgaImage?.Type}");
                        needsUpdate = true;
                    }
                    else if (required.FpgaImage?.Version != "0.0.0.0")
                    {
                        // Version 0.0.0.0 means "any version allowed"
                        // Otherwise check if loaded version is older than required version
                        // Newer versions are always acceptable and should not trigger an update
                        int versionComparison = CompareVersions(loaded.FpgaImage?.Version, required.FpgaImage?.Version);
                        if (versionComparison < 0)
                        {
                            // Loaded version is older than required - needs update
                            if (EnableDebugOutput)
                                Utils.WriteLine($"[DEBUG] FPGA ID={required.Id} version too old: required={required.FpgaImage?.Version}, loaded={loaded.FpgaImage?.Version}");
                            needsUpdate = true;
                        }
                        else if (EnableDebugOutput && versionComparison > 0)
                        {
                            // Loaded version is newer - this is acceptable
                            Utils.WriteLine($"[DEBUG] FPGA ID={required.Id} has newer version: required={required.FpgaImage?.Version}, loaded={loaded.FpgaImage?.Version} (OK)");
                        }
                    }
                    
                    if (needsUpdate)
                    {
                        if (EnableDebugOutput)
                            Utils.WriteLine($"[DEBUG] FPGA ID={required.Id} needs update");
                        missingUpdates.Add(required);
                    }
                    else
                    {
                        if (EnableDebugOutput)
                            Utils.WriteLine($"[DEBUG] FPGA ID={required.Id} is compatible");
                    }
                }

                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] Total FPGAs requiring update: {missingUpdates.Count}");

                if (missingUpdates.Count > 0)
                {
                    // Build warning message with required FPGA details
                    var fpgaDetails = string.Join(", ", missingUpdates.Select(f =>
                        $"Type {f.FpgaImage?.Type} (v{f.FpgaImage?.Version})"));

                    Utils.WriteLine($"(!) FPGA Update required: {fpgaDetails}");
                    
                    // Try to find and return FPGA container file for auto-update
                    return await FindFpgaContainerForCliAsync(missingUpdates, s19FilePath);
                }
                else
                {
                    // All compatible
                    Utils.WriteLine("(+) FPGA compatibility check: OK");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLine($"(x) FPGA check error: {ex.Message}");
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] Exception stack: {ex.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Finds FPGA container file for CLI auto-update
        /// </summary>
        private static async Task<string?> FindFpgaContainerForCliAsync(List<FpgaRequirement> missingUpdates, string? s19FilePath)
        {
            try
            {
                if(EnableDebugOutput)
                    Utils.WriteLine("(i) Searching for FL3X Config installation...");
                string? fl3xPath = FL3XConfigScanner.FindNewestInstallation(null);
                
                if (string.IsNullOrEmpty(fl3xPath))
                {
                    Utils.WriteLine("(x) FL3X Config not found, cannot auto-update FPGA images");
                    return null;
                }

                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] Found FL3X Config: {fl3xPath}");

                string? variantsPath = FL3XConfigScanner.GetFpgaVariantsPath(fl3xPath, null);
                if (string.IsNullOrEmpty(variantsPath))
                {
                    Utils.WriteLine("(x) FL3X Config Variants folder not found");
                    return null;
                }

                Utils.WriteLine("(i) Searching for matching FPGA container file...");
                string? containerFile = FpgaFileFinder.FindContainerFile(missingUpdates, s19FilePath, variantsPath, null);
                
                if (string.IsNullOrEmpty(containerFile))
                {
                    var typesList = string.Join(", ", missingUpdates.Select(f => f.FpgaImage?.Type));
                    Utils.WriteLine($"(x) No matching FPGA container file found for types: {typesList}");
                    return null;
                }

                string filename = System.IO.Path.GetFileName(containerFile);
                Utils.WriteLine($"(+) Found FPGA container: {filename}");
                return containerFile;
            }
            catch (Exception ex)
            {
                Utils.WriteLine($"(x) Failed to find FPGA container: {ex.Message}");
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] FindFpgaContainerForCliAsync exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Triggers automatic FPGA update if enabled
        /// </summary>
        private static void TriggerAutoUpdate(
            List<FpgaRequirement> missingUpdates,
            string ipAddress,
            OutputBox outputBox,
            string? s19FilePath,
            FlexTftpForm? form)
        {  
            if (!Properties.Settings.Default.AutoFpgaUpdate)
            {
                if (EnableDebugOutput)
                    outputBox.AddLine("[DEBUG] AutoUpdate: Auto-update disabled in settings", Color.Gray, true);
                return;
            }

            if (form == null)
            {
                if (EnableDebugOutput)
                    outputBox.AddLine("[DEBUG] AutoUpdate: No form reference provided", Color.Gray, true);
                return;
            }

            try
            {
                if(EnableDebugOutput)
                    outputBox.AddLine("Searching for FL3X Config installation...", Color.Gray, true);
                string? fl3xPath = FL3XConfigScanner.FindNewestInstallation(outputBox);
                
                if (string.IsNullOrEmpty(fl3xPath))
                {
                    outputBox.AddLine("FL3X Config not found, cannot auto-update FPGA images", Color.DarkRed, true);
                    return;
                }

                if(EnableDebugOutput)
                    outputBox.AddLine($"Found FL3X Config: {fl3xPath}", Color.Gray, true);

                string? variantsPath = FL3XConfigScanner.GetFpgaVariantsPath(fl3xPath, outputBox);
                if (string.IsNullOrEmpty(variantsPath))
                {
                    outputBox.AddLine("FL3X Config Variants folder not found", Color.DarkRed, true);
                    return;
                }

                if(EnableDebugOutput)
                    outputBox.AddLine("Searching for matching FPGA container file...", Color.Gray, true);
                string? containerFile = FpgaFileFinder.FindContainerFile(missingUpdates, s19FilePath, variantsPath, outputBox);
                
                if (string.IsNullOrEmpty(containerFile))
                {
                    var typesList = string.Join(", ", missingUpdates.Select(f => f.FpgaImage?.Type));
                    outputBox.AddLine($"No matching FPGA file found for: {typesList}", Color.DarkRed, true);
                    return;
                }

                string filename = System.IO.Path.GetFileName(containerFile);
                outputBox.AddLine($"Found FPGA container: {filename}", Color.Black, true);
                outputBox.AddLine("Starting automatic FPGA update...", Color.Black, true);

                // Start transfer on UI thread
                form.Invoke(new Action(() =>
                {
                    try
                    {
                        form.Transfer.StartTransfer(containerFile, "fpga/application", ipAddress, 69);
                    }
                    catch (Exception ex)
                    {
                        outputBox.AddLine($"Failed to start FPGA update: {ex.Message}", Color.Red, true);
                    }
                }));
            }
            catch (Exception ex)
            {
                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] AutoUpdate: Exception: {ex.Message}", Color.Red, true);
                outputBox.AddLine($"Auto-update failed: {ex.Message}", Color.DarkRed, true);
            }
        }

        /// <summary>
        /// Waits for device to respond to ping
        /// </summary>
        private static bool WaitForDeviceOnline(string ipAddress, OutputBox outputBox)
        {
            var endTime = DateTime.Now.AddSeconds(MaxWaitTimeSeconds);
            int attemptCount = 0;

            while (DateTime.Now < endTime)
            {
                attemptCount++;
                try
                {
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] WaitForDeviceOnline: Ping attempt #{attemptCount}", Color.Gray, true);
                    
                    using (var ping = new Ping())
                    {
                        var reply = ping.Send(ipAddress, 1000);
                        if (EnableDebugOutput)
                            outputBox.AddLine($"[DEBUG] WaitForDeviceOnline: Reply status: {reply?.Status}", Color.Gray, true);
                        
                        if (reply?.Status == IPStatus.Success)
                        {
                            outputBox.AddLine("Device is online, checking FPGA compatibility...", Color.Gray, true);
                            if (EnableDebugOutput)
                                outputBox.AddLine($"[DEBUG] WaitForDeviceOnline: Device online after {attemptCount} attempts", Color.Gray, true);
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] WaitForDeviceOnline: Ping exception: {ex.Message}", Color.Gray, true);
                    // Ping failed, continue waiting
                }

                // Wait before next ping
                Thread.Sleep(PingIntervalSeconds * 1000);
            }

            if (EnableDebugOutput)
                outputBox.AddLine($"[DEBUG] WaitForDeviceOnline: Timeout after {attemptCount} attempts", Color.Gray, true);
            return false;
        }

        /// <summary>
        /// Gets required FPGA images from /api/project
        /// </summary>
        private static List<FpgaRequirement>? GetRequiredFpgas(string ipAddress, OutputBox outputBox)
        {
            try
            {
                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] GetRequiredFpgas: Creating HttpClient", Color.Gray, true);
                
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds);
                    client.DefaultRequestHeaders.Add("user-agent", "FlexTFTP");

                    var url = $"http://{ipAddress}/api/project";
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] GetRequiredFpgas: Calling {url}", Color.Gray, true);
                    
                    var responseTask = client.GetStringAsync(url);
                    responseTask.Wait();
                    var response = responseTask.Result;

                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] GetRequiredFpgas: Response length: {response?.Length ?? 0}", Color.Gray, true);

                    var projectData = JsonConvert.DeserializeObject<ProjectResponse>(response);
                    
                    if (projectData?.Config?.Devices == null)
                    {
                        if (EnableDebugOutput)
                            outputBox.AddLine($"[DEBUG] GetRequiredFpgas: projectData.Config.Devices is null", Color.Gray, true);
                        return null;
                    }

                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] GetRequiredFpgas: Found {projectData.Config.Devices.Count} device(s)", Color.Gray, true);

                    // Collect all FPGAs from all devices
                    var allFpgas = new List<FpgaRequirement>();
                    foreach (var device in projectData.Config.Devices)
                    {
                        if (device.Fpgas != null)
                        {
                            if (EnableDebugOutput)
                                outputBox.AddLine($"[DEBUG] GetRequiredFpgas: Device has {device.Fpgas.Count} FPGA(s)", Color.Gray, true);
                            allFpgas.AddRange(device.Fpgas);
                        }
                    }

                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] GetRequiredFpgas: Returning {allFpgas.Count} total FPGA(s)", Color.Gray, true);
                    return allFpgas;
                }
            }
            catch (Exception ex)
            {
                if (EnableDebugOutput)
                {
                    outputBox.AddLine($"[DEBUG] GetRequiredFpgas: Exception: {ex.Message}", Color.Red, true);
                    if (ex.InnerException != null)
                    {
                        outputBox.AddLine($"[DEBUG] GetRequiredFpgas: InnerException: {ex.InnerException.Message}", Color.Red, true);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets loaded FPGA images from /api/device/fpga
        /// </summary>
        private static List<LoadedFpga>? GetLoadedFpgas(string ipAddress, OutputBox outputBox)
        {
            try
            {
                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] GetLoadedFpgas: Creating HttpClient", Color.Gray, true);
                
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds);
                    client.DefaultRequestHeaders.Add("user-agent", "FlexTFTP");

                    var url = $"http://{ipAddress}/api/device/fpga";
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] GetLoadedFpgas: Calling {url}", Color.Gray, true);
                    
                    var responseTask = client.GetStringAsync(url);
                    responseTask.Wait();
                    var response = responseTask.Result;

                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] GetLoadedFpgas: Response length: {response?.Length ?? 0}", Color.Gray, true);

                    var loadedFpgas = JsonConvert.DeserializeObject<List<LoadedFpga>>(response);
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] GetLoadedFpgas: Returning {loadedFpgas?.Count ?? 0} FPGA(s)", Color.Gray, true);
                    return loadedFpgas;
                }
            }
            catch (Exception ex)
            {
                if (EnableDebugOutput)
                {
                    outputBox.AddLine($"[DEBUG] GetLoadedFpgas: Exception: {ex.Message}", Color.Red, true);
                    if (ex.InnerException != null)
                    {
                        outputBox.AddLine($"[DEBUG] GetLoadedFpgas: InnerException: {ex.InnerException.Message}", Color.Red, true);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Waits for device to respond to ping (CLI version)
        /// </summary>
        private static bool WaitForDeviceOnlineCli(string ipAddress)
        {
            return PingDevice.Ping(ipAddress, MaxWaitTimeSeconds, 3);
        }

        /// <summary>
        /// Gets required FPGA images from /api/project (CLI version)
        /// </summary>
        private static async Task<List<FpgaRequirement>?> GetRequiredFpgasCliAsync(string ipAddress)
        {
            try
            {
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: Creating HttpClient");
                
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds);
                    client.DefaultRequestHeaders.Add("user-agent", "FlexTFTP");

                    var url = $"http://{ipAddress}/api/project";
                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: Calling {url}");
                    
                    var response = await client.GetStringAsync(url);

                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: Response length: {response?.Length ?? 0}");

                    var projectData = JsonConvert.DeserializeObject<ProjectResponse>(response);

                    if (projectData?.Config?.Devices == null)
                    {
                        if (EnableDebugOutput)
                            Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: projectData.Config.Devices is null");
                        return null;
                    }

                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: Found {projectData.Config.Devices.Count} device(s)");

                    // Collect all FPGAs from all devices
                    var allFpgas = new List<FpgaRequirement>();
                    foreach (var device in projectData.Config.Devices)
                    {
                        if (device.Fpgas != null)
                        {
                            if (EnableDebugOutput)
                                Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: Device has {device.Fpgas.Count} FPGA(s)");
                            allFpgas.AddRange(device.Fpgas);
                        }
                    }

                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: Returning {allFpgas.Count} total FPGA(s)");
                    return allFpgas;
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLine($"(x) Failed to get required FPGAs: {ex.Message}");
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] GetRequiredFpgasCliAsync: Exception details: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Gets loaded FPGA images from /api/device/fpga (CLI version)
        /// </summary>
        private static async Task<List<LoadedFpga>?> GetLoadedFpgasCliAsync(string ipAddress)
        {
            try
            {
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] GetLoadedFpgasCliAsync: Creating HttpClient");
                
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds);
                    client.DefaultRequestHeaders.Add("user-agent", "FlexTFTP");

                    var url = $"http://{ipAddress}/api/device/fpga";
                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] GetLoadedFpgasCliAsync: Calling {url}");
                    
                    var response = await client.GetStringAsync(url);

                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] GetLoadedFpgasCliAsync: Response length: {response?.Length ?? 0}");

                    var loadedFpgas = JsonConvert.DeserializeObject<List<LoadedFpga>>(response);
                    
                    if (EnableDebugOutput)
                        Utils.WriteLine($"[DEBUG] GetLoadedFpgasCliAsync: Returning {loadedFpgas?.Count ?? 0} FPGA(s)");
                    
                    return loadedFpgas;
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLine($"(x) Failed to get loaded FPGAs: {ex.Message}");
                if (EnableDebugOutput)
                    Utils.WriteLine($"[DEBUG] GetLoadedFpgasCliAsync: Exception details: {ex}");
                return null;
            }
        }

        #region JSON Response Classes

        private class ProjectResponse
        {
            [JsonProperty("config")]
            public ConfigData? Config { get; set; }
        }

        private class ConfigData
        {
            [JsonProperty("devices")]
            public List<DeviceConfig>? Devices { get; set; }
        }

        private class DeviceConfig
        {
            [JsonProperty("fpgas")]
            public List<FpgaRequirement>? Fpgas { get; set; }
        }

        public class FpgaRequirement
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("fpgaImage")]
            public FpgaImageInfo? FpgaImage { get; set; }
        }

        private class LoadedFpga
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("fpgaImage")]
            public FpgaImageInfo? FpgaImage { get; set; }

            [JsonProperty("projectCompatibility")]
            public string? ProjectCompatibility { get; set; }
        }

        public class FpgaImageInfo
        {
            [JsonProperty("version")]
            public string? Version { get; set; }

            [JsonProperty("type")]
            public string? Type { get; set; }
        }

        #endregion

        /// <summary>
        /// Compares two version strings
        /// </summary>
        /// <param name="version1">First version string (e.g., "1.2.3.4")</param>
        /// <param name="version2">Second version string (e.g., "1.2.3.4")</param>
        /// <returns>
        /// -1 if version1 is older than version2,
        /// 0 if versions are equal,
        /// 1 if version1 is newer than version2
        /// </returns>
        private static int CompareVersions(string? version1, string? version2)
        {
            // Handle null or empty cases
            if (string.IsNullOrEmpty(version1) && string.IsNullOrEmpty(version2))
                return 0;
            if (string.IsNullOrEmpty(version1))
                return -1; // No version is considered older
            if (string.IsNullOrEmpty(version2))
                return 1;

            try
            {
                // Try to parse as Version objects for proper comparison
                var v1 = new Version(version1);
                var v2 = new Version(version2);
                return v1.CompareTo(v2);
            }
            catch
            {
                // Fallback to string comparison if parsing fails
                return string.Compare(version1, version2, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
