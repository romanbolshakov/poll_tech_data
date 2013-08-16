using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eam.Client.Model.TechData.OPCDataContract;


namespace Eam.Client.Model.TechData {
    internal abstract class TDOpcPollProcess: TDPollProcess {
        protected OPCDA.NET.OpcServer _coreOpcServer;
        protected string[] _opcItemNames;
        protected bool _isServerConnected;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="opcServer">OPC server instance (intrenal)</param>
        /// <param name="currentDataManager">Cerrent data manager</param>
        /// <param name="pollDelay">Poll delay in milliseconds (500 ms by default)</param>
        protected TDOpcPollProcess(OPCServer opcServer, TDDataManager currentDataManager, int pollDelay) :
            base(currentDataManager, pollDelay, opcServer.FullNetworkName) {
            _isServerConnected = ConnectToServer(opcServer.HostName, opcServer.ServerName);
            _opcItemNames = GetOpcItemNames(opcServer);
            IList<CommonDataContract.PollItem> pollItems = GetPollItems(opcServer);
            currentDataManager.RegisterPollItems(pollItems);
        }

        protected CommonDataContract.PollItemValue[] ConvertToPollItemValues(OPCDA.NET.ItemValue[] values) {
            List<CommonDataContract.PollItemValue> result = new List<CommonDataContract.PollItemValue>();
            OPCDataContract.OPCItemValue newItemValue;
            foreach (var currentOPCItemValue in values) {
                newItemValue = new OPCDataContract.OPCItemValue(currentOPCItemValue);
                result.Add(newItemValue);
            }
            return result.ToArray();
        }

        private bool ConnectToServer(string hostName, string serverName) {
            try {
                _coreOpcServer = new OPCDA.NET.OpcServer();
                int hresult = _coreOpcServer.Connect(@"\\" + hostName + "\\" + serverName);
                if (!OPC.HRESULTS.Failed(hresult)) {
                    return true;
                }
                return false;
            }
            catch(Exception ex) {
                TDInternalLogger.GetLogger().LogException(new Exception("OPC Server connection failed", ex));
                return false;
            }
        }

        private string[] GetOpcItemNames(OPCServer opcServer) {
            List<string> result = new List<string>();
            foreach (var currentGroup in opcServer.Groups) {
                foreach (var currentItem in currentGroup.Items) {
                    result.Add(currentItem.ItemName);
                }
            }
            return result.ToArray();
        }

        private List<CommonDataContract.PollItem> GetPollItems(OPCServer opcServer) {
            List<CommonDataContract.PollItem> result = new List<CommonDataContract.PollItem>();
            foreach (var currentGroup in opcServer.Groups) {
                foreach (var currentItem in currentGroup.Items) {
                    result.Add(currentItem);
                }
            }
            return result;
        }
    }
}
