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

        private System.Data.SqlServerCe.SqlCeConnection _connection;

        public InternalDataStore() {
            _connection = 
                new System.Data.SqlServerCe.SqlCeConnection("Data Source=./TechDatabase.sdf");
            _connection.Open();
        }
        
        public void SaveValues(CommonDataContract.PollItemValue[] pollItemValues) {
            System.Data.SqlServerCe.SqlCeCommand cmd = new System.Data.SqlServerCe.SqlCeCommand();
            cmd.Connection = _connection;
            cmd.CommandType = System.Data.CommandType.Text;
            foreach (var currentPollItemValue in pollItemValues) {
                cmd.CommandText = String.Format("insert into item_values (itemID, Timestamp, Value) values ('{0}','{1}','{2}')",
                    currentPollItemValue.ItemID, currentPollItemValue.Timestamp, currentPollItemValue.Value);
                int rowCount = cmd.ExecuteNonQuery();
            }
        }
    }
}
