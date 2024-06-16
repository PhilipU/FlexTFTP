using System.Collections.Generic;

namespace FlexTFTP
{
    class ErrorMessageTranslator
    {
        private static readonly Dictionary<string, string> Dict = new Dictionary<string, string>()
        {
            { "Timeout error. RetryTimeout",                     "Host unreachable!" },
            { "System.Net.Sockets.SocketException (0x80004005)", "Connection rejected by host."},
            { "99 - Cancelled by administrator",                 "Transfer cancelled by remote host."},
            { "Tftp.Net.TftpParserException: Error while parsing message.", "Host answered with corrupt message."},
            { "0 - Invalid device type", "Wrong target device or revision."},
        };

        public static string TranslateError(string error)
        {
            foreach (KeyValuePair<string, string> entry in Dict)
            {
                if (error.StartsWith(entry.Key))
                {
                    return entry.Value;
                }
            }
            return error;
        }
    }
}
