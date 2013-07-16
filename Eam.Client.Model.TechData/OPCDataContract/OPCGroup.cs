using System;
using System.Collections.Generic;
using System.Text;

namespace Eam.Client.Model.TechData.OPCDataContract {
    public class OPCGroup {
        private List<OPCItem> _opcItems;
        public List<OPCItem> Items {
            get { return _opcItems; }
            set { _opcItems = value; }
        }

        private OPCServer _opcServer;
        public OPCServer OwnerServer {
            get { return _opcServer; }
            set { _opcServer = value; }
        }

        private string _name;
        public string GroupName {
            get { return _name; }
            set { _name = value; }
        }

        public OPCGroup() {
            _opcItems = new List<OPCItem>();
        }

        public override string ToString() {
            return _name;
        }

    }
}
