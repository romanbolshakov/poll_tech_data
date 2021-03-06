﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Eam.Client.Model.TechData;
using Eam.Client.Model.TechData.OPCDataContract;
using Eam.Client.Model.TechData.CommonDataContract;

namespace test_EamTechDataModel_console {
    class Program {

        private static bool isStop;
        private static string xmlConfigurationFileName;
        private static List<PollItem> _monitorPollItems;

        private static System.Threading.AutoResetEvent autoResetEvent;

        static void Main(string[] args) {
            TDInternalLogger.GetLogger().AddNewLogMessageEvent += new EventHandler<Eam.Client.Model.TechData.MyEventArgs.LogMessageEventArgs>(Program_AddNewLogMessageEvent);
            isStop = false;
            if (args.Length > 0) {
                xmlConfigurationFileName = args[0];
            }
            System.Threading.Thread bufferUpdateMonitor = new System.Threading.Thread(UpdateMonitor);
            bufferUpdateMonitor.IsBackground = true;
            bufferUpdateMonitor.Start();
            string input = Console.ReadLine();
            while (input != "q") {
                input = Console.ReadLine();
            }

            isStop = true;
            Console.WriteLine("Waiting for stop polling");
            autoResetEvent = new System.Threading.AutoResetEvent(false);
            autoResetEvent.WaitOne();
        }

        static void Program_AddNewLogMessageEvent(object sender, Eam.Client.Model.TechData.MyEventArgs.LogMessageEventArgs e) {
            Console.WriteLine(e.LogMessage);
        }

        private static void UpdateMonitor() {
            TDConfiguration configuration;
            _monitorPollItems = new List<PollItem>();
            if (xmlConfigurationFileName == null) {
                configuration = CreateTestConfiguration();
            }
            else {
                configuration = TDConfiguration.CreateConfigurationByXML(xmlConfigurationFileName);
            }

            FillMonitorItems(configuration);

            TDProcessManager processManager = new TDProcessManager(configuration);
            processManager.StartAllProcesses();
            DateTime lastUpdateStamp;
            lastUpdateStamp = DateTime.Now;
            string itemID;
            string itemValue;
            System.Threading.Thread.Sleep(1000);
            while (!isStop) {
                if (lastUpdateStamp != processManager.CurrentDataManager.GetLastUpdatedTimestamp) {
                    foreach (PollItem item in _monitorPollItems) {
                        try {
                            itemValue = item.GetLastValue().Value.ToString();
                            itemID = item.ItemName;
                            Console.WriteLine("{0} = {1}", itemID, itemValue);
                        }
                        catch{
                        }
                    }
                    lastUpdateStamp = processManager.CurrentDataManager.GetLastUpdatedTimestamp;
                    System.Threading.Thread.Sleep(500);
                }
            }
            processManager.StopAllProcesses();
            autoResetEvent.Set();
        }

        private static void FillMonitorItems(TDConfiguration configuration) {
            foreach (TDDataSource item in configuration.GetDataSources) {
                foreach (var opcGroup in (item as TDOpcDataSource).OpcServer.Groups) {
                    foreach (var opcItem in opcGroup.Items) {
                        _monitorPollItems.Add(opcItem);
                        if (opcItem.SubItems != null) {
                            foreach (var subItem in opcItem.SubItems) {
                                _monitorPollItems.Add(subItem);
                            }
                        }
                    }
                }
            }
        }

        private static TDConfiguration CreateTestConfiguration() {
            TDConfiguration configuration = new TDConfiguration();
            OPCServer opcServer = new OPCServer();
            opcServer.HostName = "localhost";
            opcServer.ServerName = "Advosol.SimDAServer.1";

            OPCGroup opcGroup = new OPCGroup();
            opcGroup.OwnerServer = opcServer;
            opcGroup.GroupName = "SimulatedData";
            opcServer.Groups.Add(opcGroup);

            OPCItem opcItem = new OPCItem();
            opcItem.FullName = "SimulatedData.Random";
            opcItem.ItemName = "SimulatedData.Random";

            opcGroup.Items.Add(opcItem);
            _monitorPollItems.Add(opcItem);

            TDOpcDataSource opcDataSource = new TDOpcV2DataSource();
            opcDataSource.OpcServer = opcServer;

            configuration.GetDataSources.AddDataSource(opcDataSource);
            return configuration;
            
        }

        /*private static TDConfiguration CreateConfigurationByXML(string xmlFileName) {
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
                            if (dataSourceType.ToLower() == "opc") {
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
            TDOpcDataSource opcDataSource = new TDOpcDataSource();
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
                            //opcItem.ValueUpdatedEvent += new EventHandler<PollItem.PollItemValueUpdatedEventArgs>(opcItem_ValueUpdatedEvent);
                            _monitorPollItems.Add(opcItem);
                        }
                    }
                }
            }
            return opcDataSource;
        }
        */

        static void opcItem_ValueUpdatedEvent(object sender, PollItem.PollItemValueUpdatedEventArgs e) {
            /*string itemValue = e.NewPollItemValue.Value.ToString();
            string itemID = e.NewPollItemValue.ItemID;
            Console.WriteLine("{0} = {1}", itemID, itemValue);*/
        }
    }
}
