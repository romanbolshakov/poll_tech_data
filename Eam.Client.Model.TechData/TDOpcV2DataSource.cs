using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    public class TDOpcV2DataSource: TDOpcDataSource {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pollDelay">Poll delay in milliseconds (500 by default)</param>
        public TDOpcV2DataSource(): base() {
            base.DataSourceType = TDDataSourceType.OPC_V2;
        }
    }
}
