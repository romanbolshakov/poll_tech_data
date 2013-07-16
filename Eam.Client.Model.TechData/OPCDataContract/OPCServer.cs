using System;
using System.Collections.Generic;
using System.Text;

namespace Eam.Client.Model.TechData.OPCDataContract {
    public class OPCServer {
        private List<OPCGroup> _opcGroups;
        public List<OPCGroup> Groups {
            get { return _opcGroups; }
            set { _opcGroups = value; }
        }

        private string _name;
        public string ServerName {
            get { return _name; }
            set { _name = value; }
        }

        private string _hostName;
        public string HostName {
            get { return _hostName; }
            set { _hostName = value; }
        }

        /// <summary>
        /// Return full network server name for connection
        /// (ex. \\localhost\\AdvsolSimDAServer.1)
        /// </summary>
        public string FullNetworkName {
            get {
                return String.Format(@"\\{0}\\{1}", _hostName, _name);
            }
        }

        public OPCServer() {
            _opcGroups = new List<OPCGroup>();
        }

        public override string ToString() {
            return _name;
        }
    }
}
