using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsLibrary
{
    public class Log
    {
        public string Time { get; set; }
        public LogSeverity Severity { get; set; }
        public string Message { get; set; }
        public Log(LogSeverity severity, string message)
        {
            Severity = severity;
            Message = message;
            Time = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
