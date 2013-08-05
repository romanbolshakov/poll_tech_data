using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData.InternalDAL {
    /// <summary>
    /// Class of data store implementation (IDataStore)
    /// Using if client code not provide his own implementation
    /// </summary>
    internal class InternalDataStore:Interfaces.IDataStore {
        public void SaveValues(CommonDataContract.PollItemValue[] pollItemValues) {
            
        }
    }
}
