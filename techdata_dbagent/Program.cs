using System;
using System.Collections.Generic;
using System.Text;

using Eam.Client.Model.TechData;
using Eam.Client.Model.TechData.Interfaces;
using Eam.Client.Model.TechData.OPCDataContract;
using Eam.Client.Model.TechData.CommonDataContract;

namespace techdata_dbagent {
    class Program {

        private static bool isStop;
        private static string xmlConfigurationFileName;
        private static List<PollItem> _monitorPollItems;

        private static System.Threading.AutoResetEvent autoResetEvent;

        static void Main(string[] args) {
            ShowConsoleMessage("Start program");
            isStop = false;
            if (args.Length > 0) {
                xmlConfigurationFileName = args[0];
            }
            System.Threading.Thread bufferUpdateMonitor = new System.Threading.Thread(UpdateMonitor);
            bufferUpdateMonitor.IsBackground = true;
            bufferUpdateMonitor.Start();
            Console.WriteLine("Press Q for quit");
            string input = Console.ReadLine();
            while (input != "q") {
                input = Console.ReadLine();
            }

            isStop = true;
            Console.WriteLine("Waiting for stop polling");
            autoResetEvent = new System.Threading.AutoResetEvent(false);
            autoResetEvent.WaitOne();
        }

        private static void ShowConsoleMessage(string message) {
            Console.WriteLine(String.Format("{0}: {1}", DateTime.Now.ToString(), message));
        }

        private static void UpdateMonitor() {
            TDConfiguration configuration;
            _monitorPollItems = new List<PollItem>();
            ShowConsoleMessage("Init configuration");
            if (xmlConfigurationFileName == null) {
                configuration = CreateTestConfiguration();
            }
            else {
                configuration = TDConfiguration.CreateConfigurationByXML(xmlConfigurationFileName);
            }

            FillMonitorItems(configuration);

            IDataStore dataStore = new SQLDataStore(Properties.Settings.Default.ConnectingString);
            TDProcessManager processManager = null;
            ShowConsoleMessage("Init process manager");
            processManager = new TDProcessManager(configuration, dataStore);
            ShowConsoleMessage("Start poll process");
            processManager.StartAllProcesses();
            DateTime lastUpdateStamp;
            lastUpdateStamp = DateTime.Now;
            string itemID;
            string itemValue;
            System.Threading.Thread.Sleep(1000);
            while (!isStop) {
                /*if (lastUpdateStamp != processManager.CurrentDataManager.GetLastUpdatedTimestamp) {
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
                }*/
                
                // delay 10 min
                System.Threading.Thread.Sleep(6000);
            }
            if (processManager != null) {
                processManager.StopAllProcesses();
                ShowConsoleMessage("Stop poll process");
                autoResetEvent.Set();
            }
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

    }
}


