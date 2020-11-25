using System.Windows;
using UtilsLibrary;

namespace Wally.Models
{
    public static class LogsToClipboard
    {
        public static void Run()
        {
            var logger = Logger.Instance();
            var logs = string.Empty;

            foreach (Log log in logger.Logs)
            {
                logs += $"{log.Time} - {log.Severity} - {log.Message}\n";
            }
            Clipboard.SetText(logs);
        }
    }
}
