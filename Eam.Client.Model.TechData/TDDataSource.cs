using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// This class is represent an abstract data source which can be in configuration data
    /// The PollDelay field is for store value from configuration.
    /// </summary>
    public abstract class TDDataSource {

        public enum TDDataSourceType { OPC_V2, OPC_V3, Fake };

        /// <summary>
        /// [Read only]
        /// </summary>
        public TDDataSourceType DataSourceType { get; protected set; }
        /// <summary>
        /// Poll delay in milliseconds (500 by default)
        /// </summary>
        public int PollDelay { get; set; }

    }
}
