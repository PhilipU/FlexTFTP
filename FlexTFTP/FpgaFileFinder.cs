using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlexTFTP
{
    /// <summary>
    /// Finds matching FPGA image files in FL3X Config Variants folder
    /// </summary>
    public static class FpgaFileFinder
    {
        private const bool EnableDebugOutput = true;

        /// <summary>
        /// Represents a parsed FPGA image from a filename
        /// </summary>
        public class FpgaImageEntry
        {
            public string Type { get; set; } = string.Empty;
            public Version? Version { get; set; }

            public override string ToString()
            {
                return $"Type {Type} v{Version}";
            }
        }

        /// <summary>
        /// Finds a container file that contains all required FPGA images
        /// </summary>
        /// <param name="requiredFpgas">List of required FPGA images</param>
        /// <param name="variantsPath">Path to FL3X Config Variants folder</param>
        /// <param name="outputBox">Optional OutputBox for logging</param>
        /// <returns>Path to matching container file, or null if not found</returns>
        public static string? FindContainerFile(
            List<FpgaCompatibilityChecker.FpgaRequirement> requiredFpgas,
            string variantsPath,
            OutputBox? outputBox = null)
        {
            if (requiredFpgas == null || requiredFpgas.Count == 0)
            {
                if (EnableDebugOutput)
                    outputBox?.AddLine("[DEBUG] FpgaFileFinder: No required FPGAs specified", Color.Gray, true);
                return null;
            }

            if (string.IsNullOrEmpty(variantsPath) || !Directory.Exists(variantsPath))
            {
                if (EnableDebugOutput)
                    outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Variants path not found: {variantsPath}", Color.Gray, true);
                return null;
            }

            try
            {
                // Get all .fpga files
                string[] fpgaFiles = Directory.GetFiles(variantsPath, "*.fpga*", SearchOption.TopDirectoryOnly);
                
                if (EnableDebugOutput)
                    outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Found {fpgaFiles.Length} FPGA file(s) in {variantsPath}", Color.Gray, true);

                // Parse required FPGAs
                var requiredTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var requiredVersions = new Dictionary<string, Version>(StringComparer.OrdinalIgnoreCase);

                foreach (var req in requiredFpgas)
                {
                    if (req.FpgaImage != null && !string.IsNullOrEmpty(req.FpgaImage.Type))
                    {
                        string type = req.FpgaImage.Type.ToUpperInvariant();
                        requiredTypes.Add(type);

                        if (Version.TryParse(req.FpgaImage.Version, out Version? reqVersion))
                        {
                            requiredVersions[type] = reqVersion;
                        }

                        if (EnableDebugOutput)
                            outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Required: {type} v{req.FpgaImage.Version}", Color.Gray, true);
                    }
                }

                // Search for matching container file
                foreach (string filePath in fpgaFiles)
                {
                    string filename = Path.GetFileNameWithoutExtension(filePath);
                    
                    if (EnableDebugOutput)
                        outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Checking file: {filename}", Color.Gray, true);

                    var parsedImages = ParseFpgaFileName(filename);
                    
                    if (parsedImages.Count == 0)
                    {
                        if (EnableDebugOutput)
                            outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Could not parse filename: {filename}", Color.Gray, true);
                        continue;
                    }

                    // Check if this file contains ALL required types
                    var fileTypes = new HashSet<string>(parsedImages.Select(img => img.Type.ToUpperInvariant()), StringComparer.OrdinalIgnoreCase);
                    
                    bool containsAllTypes = requiredTypes.All(reqType => fileTypes.Contains(reqType));

                    if (!containsAllTypes)
                    {
                        if (EnableDebugOutput)
                        {
                            var missingTypes = requiredTypes.Except(fileTypes).ToList();
                            outputBox?.AddLine($"[DEBUG] FpgaFileFinder: File missing types: {string.Join(", ", missingTypes)}", Color.Gray, true);
                        }
                        continue;
                    }

                    // Check versions (file version must be >= required version)
                    bool versionsMatch = true;
                    foreach (var img in parsedImages)
                    {
                        string imgType = img.Type.ToUpperInvariant();
                        if (requiredVersions.ContainsKey(imgType) && img.Version != null)
                        {
                            Version requiredVersion = requiredVersions[imgType];
                            if (img.Version < requiredVersion)
                            {
                                if (EnableDebugOutput)
                                    outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Version too old for {imgType}: {img.Version} < {requiredVersion}", Color.Gray, true);
                                versionsMatch = false;
                                break;
                            }
                        }
                    }

                    if (versionsMatch)
                    {
                        if (EnableDebugOutput)
                            outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Found matching container: {filename}", Color.Green, true);
                        return filePath;
                    }
                }

                if (EnableDebugOutput)
                    outputBox?.AddLine("[DEBUG] FpgaFileFinder: No matching container file found", Color.Gray, true);
            }
            catch (Exception ex)
            {
                if (EnableDebugOutput)
                    outputBox?.AddLine($"[DEBUG] FpgaFileFinder: Error: {ex.Message}", Color.Red, true);
            }

            return null;
        }

        /// <summary>
        /// Parses FPGA type and version from filename
        /// Patterns: "0x00002161_v0.1.0.1" or "0x1094108A_v0.0.0.8_v0.0.0.6"
        /// </summary>
        private static List<FpgaImageEntry> ParseFpgaFileName(string filename)
        {
            var images = new List<FpgaImageEntry>();

            try
            {
                // Pattern to extract types: 0x followed by hex digits
                // Types are 4 hex characters each after 0x
                var typeMatch = Regex.Match(filename, @"0x([0-9A-Fa-f]+)", RegexOptions.IgnoreCase);
                if (!typeMatch.Success)
                    return images;

                string hexString = typeMatch.Groups[1].Value;
                
                // Split the hex string into 4-character chunks (each type is 4 hex chars)
                var types = new List<string>();
                for (int i = 0; i < hexString.Length; i += 4)
                {
                    if (i + 4 <= hexString.Length)
                    {
                        types.Add("0x" + hexString.Substring(i, 4).ToUpper());
                    }
                }

                // Pattern to extract versions: v followed by semantic version
                var versionMatches = Regex.Matches(filename, @"v(\d+\.\d+\.\d+\.\d+)", RegexOptions.IgnoreCase);
                
                // Match types with versions
                for (int i = 0; i < types.Count; i++)
                {
                    var entry = new FpgaImageEntry { Type = types[i] };
                    
                    if (i < versionMatches.Count)
                    {
                        string versionString = versionMatches[i].Groups[1].Value;
                        if (Version.TryParse(versionString, out Version? version))
                        {
                            entry.Version = version;
                        }
                    }
                    
                    images.Add(entry);
                }
            }
            catch
            {
                // Return empty list on parse error
            }

            return images;
        }
    }
}
