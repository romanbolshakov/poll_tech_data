using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// Poll process Manager 
    /// (instance of this class is avaible from external client code)
    /// This is entry point
    /// </summary>
    public class TDProcessManager {

        private TDDataManager _currentDataManager;
        private List<TDPollProcess> _pollProcesses;
        private TDConfiguration _currentConfiguration;

        public TDDataManager CurrentDataManager {
            get { return _currentDataManager; }
        }

        public TDProcessManager(TDConfiguration configuration) {
            _currentConfiguration = configuration;
            _currentDataManager = new TDDataManager(new InternalDAL.InternalDataStore());
            CreatePollProcesses(configuration, _currentDataManager);
        }

        public void StartAllProcesses() {
            foreach (var currentProcess in _pollProcesses) {
                currentProcess.StartPollProcess();
            }
        }

        private void CreatePollProcesses(TDConfiguration configuration, TDDataManager currentDataManager) {
            _pollProcesses = new List<TDPollProcess>();
            TDPollProcess newProcess;
            foreach (TDDataSource currentDataSource in configuration.GetDataSources) {
                switch (currentDataSource.DataSourceType) {
                    case TDDataSource.TDDataSourceType.OPC:
                        if (currentDataSource is TDOpcDataSource) {
                            newProcess = CreateOPCPollProcess(currentDataSource as TDOpcDataSource, currentDataManager);
                            _pollProcesses.Add(newProcess);
                        }
                        break;
                }
            }
        }

        private TDPollProcess CreateOPCPollProcess(TDOpcDataSource opcDataSource, TDDataManager currentDataManager) {
            TDOpcPollProcess newOpcPollProcess = new TDOpcPollProcess(opcDataSource.OpcServer, currentDataManager, opcDataSource.PollDelay);
            return newOpcPollProcess;
        }

               
    }
}
