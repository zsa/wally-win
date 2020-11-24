using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsLibrary
{
    /// <summary>
    /// The Logger class is a singleton allowing to write / read logs
    /// </summary>
    public class Logger
    {
        private static Logger _instance = null;
        private Logger() {
            Logs = new List<Log>();
        }
        public IList<Log> Logs { get; set; }

        public string LastError { get; set; }

        public static Logger Instance()
        {
            if (_instance == null) _instance = new Logger();
            return _instance;
        }

        public bool HasError => Logs.Where((log) => log.Severity == LogSeverity.Error).Count() > 0;

        public void Log(LogSeverity severity, string message)
        {
            Logs.Add(new Log(severity, message));
            if (severity == LogSeverity.Error) LastError = message;
        }
    }
}
