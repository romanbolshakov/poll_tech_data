using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    public abstract class TDDataSource {

        public enum TDDataSourceType { OPC };

        public TDDataSourceType DataSourceType { get; private set; }

    }
}
