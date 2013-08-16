using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData.MyEventArgs {
    public class LogMessageEventArgs : EventArgs {
        private string _logMessage;
        public string LogMessage {
            get { return _logMessage; }
        }

        internal LogMessageEventArgs(string logMessage) {
            _logMessage = logMessage;
        }
    }

}
