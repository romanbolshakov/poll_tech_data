using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace techdata_dbagent {
    internal class ImportItemsFromXML {

        private XmlDocument _xmlDocument;
        private SqlConnection _sqlConnection;

        public ImportItemsFromXML(string fileName) {
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(fileName);
            _sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectingString);
        }

        internal void ImportData() {
            _sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "sp_add_new_item";
            XmlNodeList itemNodeList = _xmlDocument.SelectNodes("data-sources/data-source/group/item/sub-item");
            string itemName;
            int i = 0;
            foreach (XmlNode currentNode in itemNodeList) {
                try {
                    sqlCommand.Parameters.Clear();
                    itemName = currentNode.Attributes["name"].Value.ToString();
                    sqlCommand.Parameters.AddWithValue("@name", itemName);
                    sqlCommand.ExecuteNonQuery();
                    Console.WriteLine(String.Format("Add new item: {0}", itemName));
                    i++;
                }
                catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine(String.Format("Import is finished. Total created items is {0}", i.ToString()));
        }
    }
}
