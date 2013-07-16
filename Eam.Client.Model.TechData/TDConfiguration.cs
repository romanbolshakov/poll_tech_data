using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eam.Client.Model.TechData.OPCDataContract;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// Class of collecting tech data configuration
    /// It consists the information about current OPCItems which must been monitoring
    /// (instance of this class is incoming from external client code)
    /// </summary>
    public class TDConfiguration {

        private TDDataSourceCollection _dataSourceCollection;
        public TDDataSourceCollection GetDataSources {
            get { return _dataSourceCollection; }
        }

        public TDConfiguration() {
            _dataSourceCollection = new TDDataSourceCollection();
        }
    }
}
