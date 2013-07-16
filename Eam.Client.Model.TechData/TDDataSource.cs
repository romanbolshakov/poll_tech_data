using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    public abstract class TDDataSource {

        public enum TDDataSourceType { OPC };

        public TDDataSourceType DataSourceType { get; private set; }
        /// <summary>
        /// Poll delay in milliseconds (500 by default)
        /// </summary>
        public int PollDelay { get; set; }

    }
}
