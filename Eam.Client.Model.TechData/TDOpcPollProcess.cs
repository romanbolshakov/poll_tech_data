using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eam.Client.Model.TechData.OPCDataContract;

namespace Eam.Client.Model.TechData {
    internal class TDOpcPollProcess: TDPollProcess {
        private OPCDA.NET.OpcServer _coreOpcServer;
        private string[] _opcItemNames;
        private bool _isServerConnected;

        internal TDOpcPollProcess(OPCServer opcServer, TDDataManager currentDataManager) :
            base(currentDataManager) {
            _isServerConnected = ConnectToServer(opcServer.HostName, opcServer.ServerName);
            _opcItemNames = GetOpcItemNames(opcServer);
        }

        protected override CommonDataContract.PollItemValue[] Poll() {
            try {
                // maxAge - это насколько "старое" в милисекундах может быть значение, если 0 то допускается только значение "от устройства"
                // в противном случае значение может быть получено из кэш'а
                OPCDA.NET.ItemValue[] values = _coreOpcServer.Read(_opcItemNames, 10);
                CommonDataContract.PollItemValue[] pollItemValues = ConvertToPollItemValues(values);
                return pollItemValues;
            }
            catch {
                return null;
            }
        }

        private CommonDataContract.PollItemValue[] ConvertToPollItemValues(OPCDA.NET.ItemValue[] values) {
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
            catch {
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
    }
}
