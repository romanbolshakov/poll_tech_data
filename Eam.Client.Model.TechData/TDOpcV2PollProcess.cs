using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eam.Client.Model.TechData.OPCDataContract;

namespace Eam.Client.Model.TechData {
    internal class TDOpcV2PollProcess: TDOpcPollProcess {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="opcServer">OPC server instance (intrenal)</param>
        /// <param name="currentDataManager">Cerrent data manager</param>
        /// <param name="pollDelay">Poll delay in milliseconds (500 ms by default)</param>
        internal TDOpcV2PollProcess(OPCServer opcServer, TDDataManager currentDataManager, int pollDelay) :
            base(opcServer, currentDataManager, pollDelay) {
        }

        protected override CommonDataContract.PollItemValue[] Poll() {
            if (_coreOpcServer.isConnectedDA) {
                OPCDA.NET.SyncIOGroup group = new OPCDA.NET.SyncIOGroup(_coreOpcServer);
                OPCDA.NET.OPCItemState opcItemState;
                List<CommonDataContract.PollItemValue> pollItemValues = new List<CommonDataContract.PollItemValue>();
                OPCDataContract.OPCItemValue newOpctemValue;
                int iRes;
                foreach (var itemName in _opcItemNames) {
                    try {
                        iRes = group.Read(OPCDA.OPCDATASOURCE.OPC_DS_CACHE, itemName, out opcItemState);
                        newOpctemValue = new OPCItemValue();
                        newOpctemValue.ItemID = itemName;
                        newOpctemValue.Value = opcItemState.DataValue;
                        newOpctemValue.Timestamp = DateTime.Now;
                        newOpctemValue.ClientHandle = opcItemState.HandleClient;
                        newOpctemValue.Error = opcItemState.Error;
                        newOpctemValue.QualityCode = opcItemState.Quality;
                        pollItemValues.Add(newOpctemValue);
                    }
                    catch (Exception ex) {
                        TDInternalLogger.GetLogger().LogException(new Exception("TDOpcV2PollProcess.Poll()", ex));
                    }

                }

                return pollItemValues.ToArray();
            }
            else {
                TDInternalLogger.GetLogger().Log("TDOpcV2PollProcess.Poll()", "DA Server not connected");
                return new CommonDataContract.PollItemValue[0];
            }
        }

    }
}
