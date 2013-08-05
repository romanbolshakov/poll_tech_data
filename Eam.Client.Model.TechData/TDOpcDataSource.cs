﻿using System;
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

        /// <summary>
        /// .ctor (poll delay = 500 ms by default)
        /// </summary>
        public TDOpcDataSource(): this(500) {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pollDelay">Poll delay in milliseconds (500 by default)</param>
        public TDOpcDataSource(int pollDelay) {
            base.PollDelay = pollDelay;
            DataSourceType = TDDataSourceType.OPC;
        }

    }
}
