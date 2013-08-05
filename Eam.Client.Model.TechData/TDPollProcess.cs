﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// The base class for common poll process
    /// It represent a common algorythm of poll process 
    /// and run it into separated thread;
    /// Inherited classes must override a Poll() method
    /// </summary>
    internal abstract class TDPollProcess {

        private TDDataManager _currentDataManager;
        private System.Threading.Thread _pollThread;
        private bool _threadStopWork;
        private int _pollDelay;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="currentDataManager">data manager instance</param>
        public TDPollProcess(TDDataManager currentDataManager) {
            _currentDataManager = currentDataManager;
            _pollDelay = 500;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="currentDataManager">data manager instance</param>
        /// <param name="pollDelay">poll delay in milliseconds (500 ms by default)</param>
        public TDPollProcess(TDDataManager currentDataManager, int pollDelay)
            : this(currentDataManager) {
            _pollDelay = pollDelay;
        }

        public void StartPollProcess() {
            _threadStopWork = false;
            _pollThread = new System.Threading.Thread(WorkPollProcess);
            _pollThread.IsBackground = true;
            _pollThread.Start();
        }

        private void WorkPollProcess() {
            CommonDataContract.PollItemValue[] pollItemValues;
            while (!_threadStopWork) {
                pollItemValues = Poll();
                if (pollItemValues != null) {
                    SaveDataToBuffer(pollItemValues);
                }
                System.Threading.Thread.Sleep(_pollDelay);
            }
        }

        private void SaveDataToBuffer(CommonDataContract.PollItemValue[] pollItemValues) {
            _currentDataManager.UpdateValues(pollItemValues);
        }

        protected abstract CommonDataContract.PollItemValue[] Poll();
    }
}
