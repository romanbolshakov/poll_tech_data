using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Eam.Client.Model.TechData {
    internal class TDInternalLogger {

        private static TDInternalLogger _instance;

        internal static TDInternalLogger GetLogger() {
            if (_instance == null) {
                _instance = new TDInternalLogger();
            }
            return _instance;
        }

        private string _logBasePath = ".\\Log\\";
        private string _logFileName = "InternalEvents.log";
        private FileStream _logFileStream;
        private StreamWriter _logStreamWriter;

        internal TDInternalLogger() {
            CreateLogFile();
        }

        internal void Log(string source, string message) {
            lock (_logStreamWriter) {
                string logMessage = String.Format("{0} {1}\t{2}\t{3}",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToShortTimeString(),
                    source, message);
                _logStreamWriter.WriteLine(logMessage);
                _logStreamWriter.Flush();
            }
        }

        internal void LogException(Exception ex) {
            Exception ownerException = ex;
            Exception innerException = ownerException.InnerException;
            int i = 0;
            string message = ownerException.Message;
            if (innerException != null) {
                do {
                    message += "\n";
                    for (int j = 0; j <= i; j++) {
                        message += "\t";
                    }
                    message += innerException.Message;
                    ownerException = innerException;
                    innerException = ownerException.InnerException;
                    i++;
                }
                while (innerException != null);
            }
            Log(ex.Source, message);
        }

        private void CreateLogFile() {
            _logFileStream = new FileStream(_logBasePath + _logFileName, FileMode.Append, FileAccess.Write);
            _logStreamWriter = new StreamWriter(_logFileStream);
        }

    }
}
