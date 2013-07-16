using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    public class TDOpcDataSource: TDDataSource {

        private OPCDataContract.OPCServer _opcServer;

        public OPCDataContract.OPCServer OpcServer {
            get { return _opcServer; }
            set { _opcServer = value; }
        }

    }
}
