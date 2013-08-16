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

        public static TDConfiguration CreateConfigurationByXML(string xmlFileName) {
            TDConfiguration configuration = new TDConfiguration();

            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(xmlFileName);

            string dataSourceType;
            TDDataSource dataSourceInstance;
            foreach (System.Xml.XmlNode dataSourcesNode in xmlDocument.ChildNodes) {
                if (dataSourcesNode.Name == "data-sources") {
                    foreach (System.Xml.XmlNode dataSourceNode in dataSourcesNode) {
                        if (dataSourceNode.Name == "data-source") {
                            dataSourceType = dataSourceNode.Attributes["type"].Value.ToString();
                            if (dataSourceType.ToLower().Substring(0,3) == "opc") {
                                dataSourceInstance = CreateOPCDataSourceByXml(dataSourceNode);
                                configuration.GetDataSources.AddDataSource(dataSourceInstance);
                            }
                        }
                    }
                }
            }
            return configuration;
        }

        private static TDDataSource CreateOPCDataSourceByXml(System.Xml.XmlNode dataSourceNode) {
            TDOpcDataSource opcDataSource;

            string dataSourceType = dataSourceNode.Attributes["type"].Value.ToString();
            if (dataSourceType.ToLower() == "opc_v3") {
                opcDataSource = new TDOpcV3DataSource();
            }
            // OPC_V2 is created by default
            else {
                opcDataSource = new TDOpcV2DataSource();
            }
            OPCServer opcServer = new OPCServer();
            opcServer.HostName = dataSourceNode.Attributes["hostname"].Value.ToString();
            opcServer.ServerName = dataSourceNode.Attributes["name"].Value.ToString();
            opcDataSource.PollDelay = Convert.ToInt16(dataSourceNode.Attributes["poll-delay"].Value);
            opcDataSource.OpcServer = opcServer;
            OPCGroup opcGroup;
            OPCItem opcItem;
            foreach (System.Xml.XmlNode groupNode in dataSourceNode) {
                if (groupNode.Name == "group") {
                    opcGroup = new OPCGroup();
                    opcServer.Groups.Add(opcGroup);
                    opcGroup.GroupName = groupNode.Attributes["name"].Value.ToString();
                    foreach (System.Xml.XmlNode itemNode in groupNode.ChildNodes) {
                        if (itemNode.Name == "item") {
                            opcItem = new OPCItem();
                            opcGroup.Items.Add(opcItem);
                            opcItem.FullName = itemNode.Attributes["name"].Value.ToString();
                            bool isPack = Convert.ToBoolean(itemNode.Attributes["is-pack"].Value);
                            if (isPack) {
                                opcItem.IsPackage = true;
                                opcItem.PackLength = Convert.ToInt16(itemNode.Attributes["pack-length"].Value);
                                CreateSubItems(opcItem, itemNode);
                            }
                            
                            //opcItem.ValueUpdatedEvent += new EventHandler<PollItem.PollItemValueUpdatedEventArgs>(opcItem_ValueUpdatedEvent);
                        }
                    }
                }
            }
            return opcDataSource;
        }

        private static void CreateSubItems(CommonDataContract.PollItem parentPollItem, System.Xml.XmlNode parentItemNode) {
            CommonDataContract.SubPollItem subPollItem;
            parentPollItem.CreateSubItemCollection();
            foreach (System.Xml.XmlNode subItemNode in parentItemNode.ChildNodes) {
                if (subItemNode.Name == "sub-item") {
                    subPollItem = new CommonDataContract.SubPollItem();
                    subPollItem.ItemName = subItemNode.Attributes["name"].Value.ToString();
                    subPollItem.BitOffset = Convert.ToInt16(subItemNode.Attributes["offset"].Value);
                    subPollItem.BitCount = Convert.ToInt16(subItemNode.Attributes["count"].Value);
                    parentPollItem.SubItems.Add(subPollItem);
                }
            }
        }
    }
}
