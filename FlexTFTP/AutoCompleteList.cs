using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FlexTFTP
{
    class AutoCompleteList
    {
        private readonly TextBox _textBox;

        public AutoCompleteList(TextBox textBox)
        {
            _textBox = textBox;

            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        public void AddEntry(string entry)
        {
            // Check if entry already in list
            //-------------------------------
            foreach(string listEntry in _textBox.AutoCompleteCustomSource)
            {
                if(listEntry.Equals(entry))
                {
                    return;
                }
            }

            // Add new entry to list
            //----------------------
            _textBox.AutoCompleteCustomSource.Add(entry);
        }

        public void AddEntries(List<string> entries)
        {
            foreach(string entry in entries)
            {
                AddEntry(entry);
            }
        }

        public void RemoveEntry(string entry)
        {
            // Remove entry
            //-------------
            _textBox.AutoCompleteCustomSource.Remove(entry);
        }

        public void Clear()
        {
            // Clear list
            //-----------
            _textBox.AutoCompleteCustomSource.Clear();
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
            foreach (string entry in _textBox.AutoCompleteCustomSource)
            {
                entriesSerialized += entry + "\r\n";
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
            string entry;
            while (null != (entry = streamReader.ReadLine()))
            {
                AddEntry(entry);
            }
            streamReader.Close();
        }
    }
}
