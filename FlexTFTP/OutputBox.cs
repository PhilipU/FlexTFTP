using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FlexTFTP
{
    public class OutputBox
    {
        public struct KnownLink
        {
            public string Link;
            public string Alias;
        }

        private const int MaxLinkLength = 45;
        private const string HistoryTextFileSuffix = ".text.conf";
        private const string HistoryIndexFileSuffix = ".index.conf";

        private readonly RichTextBox _richTextBox;
        private readonly FlexTftpForm _form;
        private readonly List<KnownLink> _linkList = new List<KnownLink>();
        private readonly ToolTip _hoverTip = new ToolTip();
        private readonly Timer _toolTipTimer = new Timer();
        private bool _clearWeaponLoaded;

        public OutputBox(FlexTftpForm form, RichTextBox richTextBox)
        {
            _form = form;
            _richTextBox = richTextBox;

            // Right click menu for output box
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Clear", ContextMenu_Clear);
            _richTextBox.ContextMenu = contextMenu;
            _richTextBox.MouseMove += MouseMove;
            _richTextBox.MouseClick += MouseClick;

            _toolTipTimer.Tick += toolTipTimer_Tick;
            _toolTipTimer.Interval = 50;
        }

        public void LoadClearWaepon()
        {
            _clearWeaponLoaded = true;
        }

        public void TriggerClearWeapon()
        {
            if (_clearWeaponLoaded) 
            { 
                _clearWeaponLoaded = false; 
                Clear(); 
            }
        }

        public void Clear()
        {
            _richTextBox.Text = string.Empty;
            _linkList.Clear();
            _richTextBox.Enabled = false;
            _richTextBox.Enabled = true;
        }

        public bool PathIsKnown(string path)
        {
            foreach(KnownLink entry in _linkList)
            {
                if(entry.Link.Equals(path))
                {
                    return true;
                }
            }
            return false;
        }

        public void Add(string text)
        {
            TriggerClearWeapon();

            _richTextBox.AppendText(text);
            Utils.ScrollToBottom(_richTextBox);
        }

        public void Add(string text, Color color)
        {
            _richTextBox.SelectionStart = _richTextBox.TextLength;
            _richTextBox.SelectionLength = 0;

            _richTextBox.SelectionColor = color;
            Add(text);
            _richTextBox.SelectionColor = _richTextBox.ForeColor;
        }

        public void AddLine()
        {
            AddLine("");
        }

        public void AddLine(string line)
        {
            TriggerClearWeapon();
            _richTextBox.AppendText(line + Environment.NewLine);
            Utils.ScrollToBottom(_richTextBox);
        }

        public void AddLine(string line, bool autoScroll)
        {
            TriggerClearWeapon();
            _richTextBox.AppendText(line + Environment.NewLine);
        }

        public void AddLine(string line, Color color)
        {
            TriggerClearWeapon();
            _richTextBox.SelectionStart = _richTextBox.TextLength;
            _richTextBox.SelectionLength = 0;

            _richTextBox.SelectionColor = color;
            AddLine(line);
            _richTextBox.SelectionColor = _richTextBox.ForeColor;
        }

        public void AddLine(string line, Color color, bool time)
        {
            if(time)
            {
                AddTimeStamp();
            }
            AddLine(line, color);
        }

        public void AddLine(string line, Color color, FontStyle style, float emSize)
        {
            TriggerClearWeapon();
            _richTextBox.SelectionStart = _richTextBox.TextLength;
            _richTextBox.SelectionLength = 0;

            _richTextBox.SelectionColor = color;
            _richTextBox.SelectionFont = new Font(_richTextBox.Font.FontFamily, emSize, style);
            AddLine(line);
            _richTextBox.SelectionColor = _richTextBox.ForeColor;
        }

        public void AddLine(string line, FontFamily fontFamily)
        {
            TriggerClearWeapon();
            Font oldFont = _richTextBox.Font;
            _richTextBox.Font = new Font(fontFamily, _richTextBox.Font.Size);

            AddLine(line);

            _richTextBox.Font = oldFont;
        }

        public void AddLine(string line, FontFamily fontFamily, float emSize, bool autoScroll)
        {
            TriggerClearWeapon();
            _richTextBox.SelectionStart = _richTextBox.TextLength;
            _richTextBox.SelectionLength = 0;

            _richTextBox.SelectionFont = new Font(fontFamily, emSize);
            AddLine(line, autoScroll);
            _richTextBox.SelectionColor = _richTextBox.ForeColor;
        }

        public void AddLine(string line, float emSize, bool autoScroll)
        {
            TriggerClearWeapon();
            _richTextBox.SelectionStart = _richTextBox.TextLength;
            _richTextBox.SelectionLength = 0;

            _richTextBox.SelectionFont = new Font(_richTextBox.Font.FontFamily, emSize);
            AddLine(line, autoScroll);
            _richTextBox.SelectionColor = _richTextBox.ForeColor;
        }

        public void AddLine(string line, float emSize)
        {
            TriggerClearWeapon();
            _richTextBox.SelectionStart = _richTextBox.TextLength;
            _richTextBox.SelectionLength = 0;

            _richTextBox.SelectionFont = new Font(_richTextBox.Font.FontFamily, emSize);
            AddLine(line);
            _richTextBox.SelectionColor = _richTextBox.ForeColor;
        }

        public void AddFilePath(string path)
        {
            string nicePath = "";

            // Check if alias file found
            if (File.Exists(path))
            {
                string aliasFilePath = Path.GetDirectoryName(path) + "/alias.flextftp";
                if (File.Exists(aliasFilePath))
                {
                    StreamReader streamReader = new StreamReader(aliasFilePath);
                    string entry;
                    while (null != (entry = streamReader.ReadLine()))
                    {
                        nicePath = Path.GetFileName(path) + " '" + entry + "'";
                    }
                    streamReader.Close();
                }
            }

            if(string.IsNullOrEmpty(nicePath))
            {
                nicePath = path;
            }

            if (nicePath.Length > MaxLinkLength && !Properties.Settings.Default.ShowFullPath)
            {
                nicePath = nicePath.Substring(nicePath.Length - MaxLinkLength);
            }

            string uri = "[" + (_linkList.Count + 1) + "] " + nicePath;
            KnownLink knownLink = new KnownLink
            {
                Link = path,
                Alias = nicePath
            };
            _linkList.Add(knownLink);
            AddLine(uri, Color.Blue, FontStyle.Underline, 8F);
        }

        private void AddTimeStamp()
        {
            Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] ", Color.DarkGray);
        }

        private void ContextMenu_Clear(object sender, EventArgs e)
        {
            Clear();
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if(_toolTipTimer.Enabled)
            {
                return;
            }

            KnownLink wholePath = GetFilePath(_richTextBox.GetCharIndexFromPosition(e.Location));

            if (string.IsNullOrEmpty(wholePath.Link))
            {
                if (!string.IsNullOrEmpty(_hoverTip.GetToolTip(_form)))
                {
                    _hoverTip.Hide(_form);
                }

                if (_richTextBox.Cursor != Cursors.Arrow)
                {
                    _richTextBox.Cursor = Cursors.Arrow;
                }
            }
            else
            {
                if (_richTextBox.Cursor != Cursors.Hand)
                {
                    _richTextBox.Cursor = Cursors.Hand;
                }

                if (string.IsNullOrEmpty(_hoverTip.GetToolTip(_form)))
                {
                    Point p = _richTextBox.Location;
                    _hoverTip.Show(wholePath.Link, _form, p.X + e.X + 20, p.Y + e.Y + 40, 10000);
                }
            }

            _toolTipTimer.Enabled = true;
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            KnownLink wholePath = GetFilePath(_richTextBox.GetCharIndexFromPosition(e.Location));

            if (string.IsNullOrEmpty(wholePath.Link))
            {
                return;
            }

            _form.SetFilePath(wholePath.Link);
        }

        private void toolTipTimer_Tick(object sender, EventArgs e)
        {
            _toolTipTimer.Enabled = false;
        }

        public KnownLink GetFilePath(int position)
        {
            string input = _richTextBox.Text;

            // Check if link is posible
            if(input.Length < position || input.Length < 3)
            {
                return new KnownLink();
            }

            // Detect empty hovered areas
            if(input[position] == '\n')
            {
                return new KnownLink();
            }

            // Get line start position
            int startIndex;
            for (startIndex = position; startIndex > 0; startIndex--)
            {
                char ch = input[startIndex];
                if (ch == '\n')
                {
                    startIndex++; // Fix start index
                    break;
                }
            }

            if(startIndex >= input.Length)
            {
                return new KnownLink();
            }

            // Check if line starts with path index
            if(input[startIndex] != '[')
            {
                return new KnownLink();
            }

            // Get length of number
            int digits = 0;
            while (input[startIndex + digits + 1] != ']' && digits < 4)
            {
                digits++;
            }

            string numberString = input.Substring(startIndex + 1, digits);
            int.TryParse(numberString, out var linkIndex);
            if(linkIndex == 0)
            {
                return new KnownLink();
            }

            if(_linkList.Count < linkIndex)
            {
                return new KnownLink();
            }

            return _linkList[linkIndex - 1];
        }

        public KnownLink GetLastFilePath()
        {
            if (_linkList.Count == 0)
            {
                return new KnownLink();
            }
            return _linkList.Last();
        }

        public void LoadFile(string basePath)
        {
            string textPath = basePath + HistoryTextFileSuffix;
            string indexPath = basePath + HistoryIndexFileSuffix;

            // Check if files exist
            //---------------------
            if(!File.Exists(textPath) ||
                !File.Exists(indexPath))
            {
                return;
            }

            TriggerClearWeapon();
            Clear();

            // Load text
            //----------
            _richTextBox.LoadFile(textPath);

            // Load index
            //-----------
            StreamReader streamReader = new StreamReader(indexPath);
            string entry;
            while(null != (entry = streamReader.ReadLine()))
            {
                KnownLink knownLink = new KnownLink
                {
                    Link = entry.Split('|')[0],
                    Alias = entry.Split('|')[1]
                };
                _linkList.Add(knownLink);
            }
            streamReader.Close();

            Utils.ScrollToBottom(_richTextBox);
        }

        public void SaveFile(string basePath)
        {
            string textPath = basePath + HistoryTextFileSuffix;
            string indexPath = basePath + HistoryIndexFileSuffix;

            // Create files
            //-------------
            if (!File.Exists(textPath))
            {
                FileStream file = File.Create(textPath);
                file.Close();
            }
            if (!File.Exists(indexPath))
            {
                FileStream file = File.Create(indexPath);
                file.Close();
            }

            // Save text
            //----------
            _richTextBox.SaveFile(textPath);

            // Save index
            //-----------
            string indexSerialized = "";
            foreach(KnownLink entry in _linkList)
            {
                indexSerialized += entry.Link + "|" + entry.Alias + "\r\n";
            }
            StreamWriter streamWriter = new StreamWriter(indexPath);
            streamWriter.Write(indexSerialized);
            streamWriter.Close();
        }
    }
}
