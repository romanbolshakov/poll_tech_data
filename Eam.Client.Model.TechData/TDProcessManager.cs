﻿using System;
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

        public TDProcessManager(TDConfiguration configuration)
            : this(configuration, new InternalDAL.InternalDataStore()) {
        }

        public TDProcessManager(TDConfiguration configuration, Interfaces.IDataStore dataStore) {
            _currentConfiguration = configuration;
            _currentDataManager = new TDDataManager(dataStore);
            CreatePollProcesses(configuration, _currentDataManager);
        }

        public void StartAllProcesses() {
            TDInternalLogger.GetLogger();
            foreach (var currentProcess in _pollProcesses) {
                currentProcess.StartPollProcess();
            }
        }

        public void StopAllProcesses() {
            foreach (var currentProcess in _pollProcesses) {
                currentProcess.StopPollProcess();
            }
        }

        private void CreatePollProcesses(TDConfiguration configuration, TDDataManager currentDataManager) {
            _pollProcesses = new List<TDPollProcess>();
            TDPollProcess newProcess;
            foreach (TDDataSource currentDataSource in configuration.GetDataSources) {
                switch (currentDataSource.DataSourceType) {
                    case TDDataSource.TDDataSourceType.OPC_V2:
                        if (currentDataSource is TDOpcDataSource) {
                            newProcess = CreateOPCV2PollProcess(currentDataSource as TDOpcDataSource, currentDataManager);
                            _pollProcesses.Add(newProcess);
                        }
                        break;
                    case TDDataSource.TDDataSourceType.OPC_V3:
                        if (currentDataSource is TDOpcDataSource) {
                            newProcess = CreateOPCV3PollProcess(currentDataSource as TDOpcDataSource, currentDataManager);
                            _pollProcesses.Add(newProcess);
                        }
                        break;
                }
            }
        }

        private TDPollProcess CreateOPCV2PollProcess(TDOpcDataSource opcDataSource, TDDataManager currentDataManager) {
            TDOpcV2PollProcess newOpcPollProcess = new TDOpcV2PollProcess(opcDataSource.OpcServer, currentDataManager, opcDataSource.PollDelay);
            return newOpcPollProcess;
        }

        private TDPollProcess CreateOPCV3PollProcess(TDOpcDataSource opcDataSource, TDDataManager currentDataManager) {
            TDOpcV3PollProcess newOpcPollProcess = new TDOpcV3PollProcess(opcDataSource.OpcServer, currentDataManager, opcDataSource.PollDelay);
            return newOpcPollProcess;
        }

               
    }
}
