using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Eam.Client.Model.TechData.Interfaces;
using Eam.Client.Model.TechData.CommonDataContract;

namespace techdata_dbagent {
    internal class SQLDataStore: IDataStore {

        private SqlConnection _sqlConnection;
        private SqlCommand _sqlCommand;
        
        internal SQLDataStore(string connectionString) {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnection;
            _sqlCommand.CommandType = CommandType.StoredProcedure;
            _sqlCommand.CommandText = "sp_save_item_value";
            _sqlCommand.Parameters.Add("@item_name", SqlDbType.VarChar);
            _sqlCommand.Parameters.Add("@item_value", SqlDbType.VarChar);
            _sqlCommand.Parameters.Add("@timestamp", SqlDbType.DateTime);
        }


        public void SaveValues(List<PollItem> updatedPollItems) {
            PollItemValue currentPollItemValue;
            foreach (var currentPollItem in updatedPollItems) {
                currentPollItemValue = currentPollItem.GetLastValue();
                _sqlCommand.Parameters["@item_name"].Value = currentPollItemValue.ItemID;
                _sqlCommand.Parameters["@item_value"].Value = currentPollItemValue.Value.ToString();
                _sqlCommand.Parameters["@timestamp"].Value = currentPollItemValue.Timestamp;
                _sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
