using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Eam.Client.Model.TechData;
using Eam.Client.Model.TechData.CommonDataContract;
using Eam.Client.Model.TechData.OPCDataContract;

namespace UnitTestProject {
    [TestClass]
    public class TDProcessManagerTests {

        [TestMethod]
        public void InitProcessManager() {
            TDConfiguration testConfiguration = CreateTestConfiguration();

            Assert.IsNotNull(testConfiguration);
            Assert.IsNotNull(testConfiguration.GetDataSources);
            Assert.IsNotNull(testConfiguration.GetDataSources[0]);
            Assert.IsTrue(testConfiguration.GetDataSources[0] is TDOpcDataSource);

            TDOpcDataSource opcDataSource = testConfiguration.GetDataSources[0] as TDOpcDataSource;
            Assert.IsNotNull(opcDataSource.OpcServer);
            Assert.AreEqual("localhost", opcDataSource.OpcServer.HostName);
            Assert.AreEqual("Advosol.SimDAServer.1", opcDataSource.OpcServer.ServerName);
            Assert.IsNotNull(opcDataSource.OpcServer.Groups);
            Assert.IsNotNull(opcDataSource.OpcServer.Groups[0]);
            Assert.IsNotNull(opcDataSource.OpcServer.Groups[0].Items);
            Assert.AreEqual("SimulatedData.Random", opcDataSource.OpcServer.Groups[0].Items[0].FullName);

            TDProcessManager processManager = new TDProcessManager(testConfiguration);
            Assert.IsNotNull(processManager.CurrentDataManager);

            processManager.StartAllProcesses();
            int i = 0;
            while (i < 100) {
                System.Threading.Thread.Sleep(100);
                i++;
            }

            PollItemValue pollItemValue = processManager.CurrentDataManager.GetLastPollItemValue("SimulatedData.Random");
            Assert.IsNotNull(pollItemValue);
            Assert.AreEqual("SimulatedData.Random", pollItemValue.ItemID);
            Assert.IsNotNull(pollItemValue.Value);

        }

        private TDConfiguration CreateTestConfiguration() {
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

            opcItem.ValueUpdatedEvent += new EventHandler<PollItem.PollItemValueUpdatedEventArgs>(opcItem_ValueUpdatedEvent);

            opcGroup.Items.Add(opcItem);

            TDOpcDataSource opcDataSource = new TDOpcV2DataSource();
            opcDataSource.OpcServer = opcServer;
            opcDataSource.PollDelay = 1000;

            configuration.GetDataSources.AddDataSource(opcDataSource);
            return configuration;
        }

        void opcItem_ValueUpdatedEvent(object sender, PollItem.PollItemValueUpdatedEventArgs e) {
            
        }
    }
}
