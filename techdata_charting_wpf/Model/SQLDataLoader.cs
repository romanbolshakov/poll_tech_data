using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace techdata_charting_wpf.Model {
    internal class SQLDataLoader: IDataLoader {

        private string _connectionString;
        private SqlConnection _sqlConnection;

        internal SQLDataLoader(string connectionString) {
            _connectionString = connectionString;
            _sqlConnection = new SqlConnection();
            _sqlConnection.ConnectionString = connectionString;
        }

        private ItemValueCollection CreateItemValues(DataTable dt) {
            ItemValueCollection result = new ItemValueCollection();
            ItemValue newItemValue;
            foreach (DataRow currentRow in dt.Rows) {
                newItemValue = CreateSingleItemValue(currentRow);
                result.Add(newItemValue);
            }
            return result;
        }

        private ItemValue CreateSingleItemValue(DataRow currentRow) {
            ItemValue itemValue = new ItemValue();
            itemValue.ItemID = currentRow["item_name"].ToString();
            itemValue.Value = currentRow["item_value"];
            itemValue.Timestamp = Convert.ToDateTime(currentRow["timestamp"]);
            return itemValue;
        }

        private ItemCollection CreateItemCollection(DataTable dt) {
            ItemCollection resultCollection = new ItemCollection();
            Item item;
            foreach (DataRow currentDataRow in dt.Rows) {
                item = new Item();
                item.ID = Convert.ToInt64(currentDataRow["ID"]);
                item.Name = currentDataRow["Name"].ToString();
                item.Caption = currentDataRow["Caption"].ToString();
                item.Description = currentDataRow["Description"].ToString();
                item.Comment = currentDataRow["Comment"].ToString();
                item.Unit = currentDataRow["UnitName"].ToString();
                resultCollection.Add(item);
            }
            return resultCollection;
        }

        private List<CustomItemCollection> CreateCustomCollectionList(DataTable dt) {
            List<Model.CustomItemCollection> resultList = new List<CustomItemCollection>();
            Model.CustomItemCollection newCustomCollection;
            foreach (DataRow currentDataRow in dt.Rows) {
                newCustomCollection = new CustomItemCollection();
                newCustomCollection.ID = Convert.ToInt16(currentDataRow["ID"]);
                newCustomCollection.Name = currentDataRow["Name"].ToString();
                newCustomCollection.Description = currentDataRow["Description"].ToString();
                resultList.Add(newCustomCollection);
            }
            return resultList;
        }

        #region IDataLoader

        public ItemValueCollection LoadItemValueCollection(string itemName) {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandText = "sp_get_values_by_item";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@item_name", itemName);

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ItemValueCollection resultCollection = CreateItemValues(dt);
            return resultCollection;
        }

        public ItemValueCollection LoadItemValueCollection(string itemName, DateTime dateFrom, DateTime? dateTo) {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandText = "sp_get_values_by_item";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@item_name", itemName);
            sqlCommand.Parameters.AddWithValue("@date_from", dateFrom.ToString());
            if (dateTo == null)
                sqlCommand.Parameters.AddWithValue("@date_to", null);
            else
                sqlCommand.Parameters.AddWithValue("@date_to", dateTo.ToString());

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ItemValueCollection resultCollection = CreateItemValues(dt);
            return resultCollection;
        }

        public ItemValueCollection LoadItemValueCollection(string itemName, DateTime lastTimestamp) {
            return LoadItemValueCollection(itemName, lastTimestamp, null);
        }

        public ItemCollection LoadAllItemCollection() {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandText = "sp_get_items";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return CreateItemCollection(dt);
        }

        public ItemCollection LoadItemCollection(int collectionID) {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandText = "sp_get_items_by_collection";
            sqlCommand.Parameters.AddWithValue("@collection_id", collectionID);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return CreateItemCollection(dt);
        }

        public ItemValue GetLastItemValue(string itemName, DateTime? customDateTime) {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandText = "sp_get_last_value_by_item";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@item_name", itemName);
            sqlCommand.Parameters.AddWithValue("@custom_datetime", customDateTime);

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ItemValueCollection resultCollection = CreateItemValues(dt);
            return resultCollection[0];
        }

        public List<CustomItemCollection> LoadCustomItemCollections() {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandText = "sp_get_collections";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return CreateCustomCollectionList(dt);
        }

        #endregion
    }
}
