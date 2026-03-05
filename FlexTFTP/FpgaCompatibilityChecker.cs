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
        /// <param name="form">Main form for triggering transfers</param>
        public static void CheckCompatibilityAsync(string ipAddress, OutputBox? outputBox, FlexTftpForm? form = null)
        {
            if (EnableDebugOutput)
                outputBox?.AddLine("[DEBUG] FPGA Check: Method entered", Color.Gray, true);
            
            if (string.IsNullOrEmpty(ipAddress) || outputBox == null)
            {
                if (EnableDebugOutput)
                    outputBox?.AddLine("[DEBUG] FPGA Check: Invalid parameters, exiting", Color.Gray, true);
                return;
            }

            try
            {
                // Log start of check
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
                    if (EnableDebugOutput)
                        outputBox.AddLine($"[DEBUG] FPGA Check: No required FPGAs found (null={requiredFpgas == null}, count={requiredFpgas?.Count ?? 0})", Color.Gray, true);
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
                    if (loaded == null || loaded.ProjectCompatibility?.Equals("Incompatible", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        missingUpdates.Add(required);
                    }
                }

                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] FPGA Check: Found {missingUpdates.Count} FPGA(s) requiring update", Color.Gray, true);

                if (missingUpdates.Count > 0)
                {
                    // Build warning message with required FPGA details
                    var fpgaDetails = string.Join(", ", missingUpdates.Select(f => 
                        $"Type {f.FpgaImage?.Type} (v{f.FpgaImage?.Version})"));
                    
                    var warningMessage = $"⚠️ FPGA Update required: {fpgaDetails}";
                    outputBox.AddLine(warningMessage, Color.DarkOrange, true);

                    // Trigger auto-update if enabled
                    TriggerAutoUpdate(missingUpdates, ipAddress, outputBox, form);
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
        /// Triggers automatic FPGA update if enabled
        /// </summary>
        private static void TriggerAutoUpdate(
            List<FpgaRequirement> missingUpdates,
            string ipAddress,
            OutputBox outputBox,
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
                outputBox.AddLine("Searching for FL3X Config installation...", Color.Gray, true);
                string? fl3xPath = FL3XConfigScanner.FindNewestInstallation(outputBox);
                
                if (string.IsNullOrEmpty(fl3xPath))
                {
                    outputBox.AddLine("⚠️ FL3X Config not found, cannot auto-update FPGA images", Color.Orange, true);
                    return;
                }

                outputBox.AddLine($"Found FL3X Config: {fl3xPath}", Color.Gray, true);

                string? variantsPath = FL3XConfigScanner.GetFpgaVariantsPath(fl3xPath, outputBox);
                if (string.IsNullOrEmpty(variantsPath))
                {
                    outputBox.AddLine("⚠️ FL3X Config Variants folder not found", Color.Orange, true);
                    return;
                }

                outputBox.AddLine("Searching for matching FPGA container file...", Color.Gray, true);
                string? containerFile = FpgaFileFinder.FindContainerFile(missingUpdates, variantsPath, outputBox);
                
                if (string.IsNullOrEmpty(containerFile))
                {
                    var typesList = string.Join(", ", missingUpdates.Select(f => f.FpgaImage?.Type));
                    outputBox.AddLine($"⚠️ No matching FPGA container file found for types: {typesList}", Color.Orange, true);
                    return;
                }

                string filename = System.IO.Path.GetFileName(containerFile);
                outputBox.AddLine($"Found FPGA container: {filename}", Color.Blue, true);
                outputBox.AddLine("Starting automatic FPGA update...", Color.Blue, true);

                // Start transfer on UI thread
                form.Invoke(new Action(() =>
                {
                    try
                    {
                        form.Transfer.StartTransfer(containerFile, "fpga/application", ipAddress, 69);
                    }
                    catch (Exception ex)
                    {
                        outputBox.AddLine($"⚠️ Failed to start FPGA update: {ex.Message}", Color.Red, true);
                    }
                }));
            }
            catch (Exception ex)
            {
                if (EnableDebugOutput)
                    outputBox.AddLine($"[DEBUG] AutoUpdate: Exception: {ex.Message}", Color.Red, true);
                outputBox.AddLine($"⚠️ Auto-update failed: {ex.Message}", Color.Orange, true);
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
    }
}
