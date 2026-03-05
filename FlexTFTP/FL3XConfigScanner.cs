using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FlexTFTP
{
    /// <summary>
    /// Scans for FL3X Config installations to find FPGA image files
    /// </summary>
    public static class FL3XConfigScanner
    {
        private const bool EnableDebugOutput = false;

        /// <summary>
        /// Finds the newest FL3X Config installation
        /// </summary>
        /// <param name="outputBox">Optional OutputBox for logging</param>
        /// <returns>Path to newest installation, or null if not found</returns>
        public static string? FindNewestInstallation(OutputBox? outputBox = null)
        {
            if (EnableDebugOutput)
                outputBox?.AddLine("[DEBUG] FL3XScanner: Starting search for FL3X Config", Color.Gray, true);

            // Search in both Program Files locations
            string[] searchPaths = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
            };

            Version? newestVersion = null;
            string? newestPath = null;

            foreach (string basePath in searchPaths)
            {
                if (string.IsNullOrEmpty(basePath) || !Directory.Exists(basePath))
                {
                    if (EnableDebugOutput)
                        outputBox?.AddLine($"[DEBUG] FL3XScanner: Base path not found: {basePath}", Color.Gray, true);
                    continue;
                }

                try
                {
                    string starCoopPath = Path.Combine(basePath, "StarCooperation");
                    if (!Directory.Exists(starCoopPath))
                    {
                        if (EnableDebugOutput)
                            outputBox?.AddLine($"[DEBUG] FL3XScanner: StarCooperation folder not found in {basePath}", Color.Gray, true);
                        continue;
                    }

                    string[] directories = Directory.GetDirectories(starCoopPath, "FL3X Config *", SearchOption.TopDirectoryOnly);
                    
                    if (EnableDebugOutput)
                        outputBox?.AddLine($"[DEBUG] FL3XScanner: Found {directories.Length} FL3X Config installation(s) in {starCoopPath}", Color.Gray, true);

                    foreach (string dir in directories)
                    {
                        string dirName = Path.GetFileName(dir);
                        // Extract version: "FL3X Config 1.5.0.5337" -> "1.5.0.5337"
                        string versionString = dirName.Replace("FL3X Config ", "").Trim();

                        if (Version.TryParse(versionString, out Version? version))
                        {
                            if (EnableDebugOutput)
                                outputBox?.AddLine($"[DEBUG] FL3XScanner: Found version {version} at {dir}", Color.Gray, true);

                            if (newestVersion == null || version > newestVersion)
                            {
                                newestVersion = version;
                                newestPath = dir;
                            }
                        }
                        else
                        {
                            if (EnableDebugOutput)
                                outputBox?.AddLine($"[DEBUG] FL3XScanner: Could not parse version from '{dirName}'", Color.Gray, true);
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    if (EnableDebugOutput)
                        outputBox?.AddLine($"[DEBUG] FL3XScanner: Access denied to {basePath}: {ex.Message}", Color.Gray, true);
                }
                catch (Exception ex)
                {
                    if (EnableDebugOutput)
                        outputBox?.AddLine($"[DEBUG] FL3XScanner: Error scanning {basePath}: {ex.Message}", Color.Gray, true);
                }
            }

            if (newestPath != null && EnableDebugOutput)
                outputBox?.AddLine($"[DEBUG] FL3XScanner: Newest installation: {newestPath} (v{newestVersion})", Color.Gray, true);
            else if (EnableDebugOutput)
                outputBox?.AddLine("[DEBUG] FL3XScanner: No FL3X Config installation found", Color.Gray, true);

            return newestPath;
        }

        /// <summary>
        /// Gets the path to the FPGA Variants folder
        /// </summary>
        /// <param name="installPath">FL3X Config installation path</param>
        /// <returns>Path to Variants folder, or null if not found</returns>
        public static string? GetFpgaVariantsPath(string installPath, OutputBox? outputBox = null)
        {
            if (string.IsNullOrEmpty(installPath))
                return null;

            try
            {
                string variantsPath = Path.Combine(installPath, "Plugins", "Variants");
                
                if (Directory.Exists(variantsPath))
                {
                    if (EnableDebugOutput)
                        outputBox?.AddLine($"[DEBUG] FL3XScanner: Variants folder found: {variantsPath}", Color.Gray, true);
                    return variantsPath;
                }
                else
                {
                    if (EnableDebugOutput)
                        outputBox?.AddLine($"[DEBUG] FL3XScanner: Variants folder not found: {variantsPath}", Color.Gray, true);
                }
            }
            catch (Exception ex)
            {
                if (EnableDebugOutput)
                    outputBox?.AddLine($"[DEBUG] FL3XScanner: Error accessing Variants folder: {ex.Message}", Color.Gray, true);
            }

            return null;
        }
    }
}
