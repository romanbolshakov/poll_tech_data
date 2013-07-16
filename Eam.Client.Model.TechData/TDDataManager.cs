using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// Class for technology data managing (store and access)
    /// </summary>
    internal class TDDataManager {

        private Interfaces.IDataStore _externalDataStore;

        public TDDataBuffer CurrentDataBuffer { get; private set; }

        internal TDDataManager(Interfaces.IDataStore dataStore) {
            _externalDataStore = dataStore;
            CurrentDataBuffer = new TDDataBuffer();
        }

        internal void UpdateValues(CommonDataContract.PollItemValue[] pollItemValues) {
            CurrentDataBuffer.UpdateValues(pollItemValues);
        }
    }
}
