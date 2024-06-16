using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlexTFTP
{
    class SRecord
    {
        const string SrecordTypeS0 = "S0";

        readonly string _file;

        public SRecord(string file)
        {
            _file = file;
        }

        private string HexToString(string hexString)
        {
            int i;
            Byte[] asciiBytes = new Byte[hexString.Length/2];

            for(i = 0; i < hexString.Length/2; i++)
            {
                string hexPair = hexString.Substring(2*i, 2);
                asciiBytes[i] = Convert.ToByte(hexPair, 16);
            }

            var str = Encoding.ASCII.GetString(asciiBytes);

            return str;
        }

        public string GetHeaderEntry()
        {
            string headerData = null;

            using (var fs = new FileStream(_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using (var streamReader = new StreamReader(fs))
            {

                var line = streamReader.ReadLine();
                if (line != null && line.StartsWith(SrecordTypeS0))
                {
                    headerData = HexToString(line.Substring(8, line.Length - 10));
                }
                streamReader.Close();
                fs.Close();
            }
            return headerData;
        }

        public List<string> GetHeaderPathList()
        {
            List<string> headers = new List<string>();
            string line;

            using (var fs = new FileStream(_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using (var streamReader = new StreamReader(fs))
            {

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!line.StartsWith(SrecordTypeS0)) continue;

                    var headerText = HexToString(line.Substring(8, line.Length - 10));

                    string targetPath = TargetPathParser.GetPathByName(headerText);
                    if (targetPath != null)
                    {
                        headers.Add(headerText);
                    }
                }

                streamReader.Close();
                fs.Close();
            }
            return headers;
        }
    }
}
