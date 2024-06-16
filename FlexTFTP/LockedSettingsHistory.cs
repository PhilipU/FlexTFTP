using System;
using System.Collections.Generic;
using System.IO;

namespace FlexTFTP
{
    class LockedSettings
    {
        public string Ip { get; }

        public string Port { get; }

        public bool AutoPath { get; }

        public string Path { get; }

        public string TargetPath { get; }

        public LockedSettings(string path, string targetPath, string ip, string port, bool autoPath)
        {
            Path = path;
            TargetPath = targetPath;
            Ip = ip;
            Port = port;
            AutoPath = autoPath;
        }
    }

    class LockedSettingsHistory
    {
        private readonly Dictionary<string, LockedSettings> _settings = new Dictionary<string, LockedSettings>();

        public void AddEntry(LockedSettings newSettings)
        {
            // Check if entry already in list
            //-------------------------------
            if(_settings.ContainsKey(newSettings.Path))
            {
                // Remove existing entry
                _settings.Remove(newSettings.Path);
            }

            // Add entry
            //----------
            _settings.Add(newSettings.Path, newSettings);
        }

        public void AddEntries(List<LockedSettings> entries)
        {
            foreach(LockedSettings entry in entries)
            {
                AddEntry(entry);
            }
        }

        public LockedSettings GetEntry(string path)
        {
            if (_settings.TryGetValue(path, out var lockedSettings))
            {
                return lockedSettings;
            }

            return null;
        }

        public void RemoveEntry(string path)
        {
            // Remove entry
            //-------------
            _settings.Remove(path);
        }

        public void Clear()
        {
            // Clear list
            //-----------
            _settings.Clear();
        }

        public void SaveFile(string filePath)
        {
            // Create files
            //-------------
            if (!File.Exists(filePath))
            {
                FileStream file = File.Create(filePath);
                file.Close();
            }

            // Save entries
            //-------------
            string entriesSerialized = "";
            foreach (KeyValuePair<string, LockedSettings> entry in _settings)
            {
                entriesSerialized += entry.Value.Path + "|" +
                                     entry.Value.TargetPath + "|" +
                                     entry.Value.Ip + "|" +
                                     entry.Value.Port + "|" +
                                     entry.Value.AutoPath + "\r\n";
            }

            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(entriesSerialized);
            streamWriter.Close();
        }

        public void LoadFile(string filePath)
        {
            // Check if files exist
            //---------------------
            if (!File.Exists(filePath))
            {
                return;
            }

            // Load entries
            //-------------
            StreamReader streamReader = new StreamReader(filePath);
            string entrySerialized;
            while (null != (entrySerialized = streamReader.ReadLine()))
            {
                var parts = entrySerialized.Split('|');

                LockedSettings newSettings = 
                    new LockedSettings(
                        parts[0], 
                        parts[1], 
                        parts[2],
                        parts[3],
                        Convert.ToBoolean(parts[4]));

                AddEntry(newSettings);
            }
            streamReader.Close();
        }
    }
}
