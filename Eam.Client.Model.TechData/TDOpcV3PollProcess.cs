using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eam.Client.Model.TechData.OPCDataContract;

namespace Eam.Client.Model.TechData {
    internal class TDOpcV3PollProcess: TDOpcPollProcess {
         /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="opcServer">OPC server instance (intrenal)</param>
        /// <param name="currentDataManager">Cerrent data manager</param>
        /// <param name="pollDelay">Poll delay in milliseconds (500 ms by default)</param>
        internal TDOpcV3PollProcess(OPCServer opcServer, TDDataManager currentDataManager, int pollDelay) :
            base(opcServer, currentDataManager, pollDelay) {
        }

        protected override CommonDataContract.PollItemValue[] Poll() {
            try {
                // maxAge - это насколько "старое" в милисекундах может быть значение, если 0 то допускается только значение "от устройства"
                // в противном случае значение может быть получено из кэш'а
                OPCDA.NET.ItemValue[] values = _coreOpcServer.Read(_opcItemNames, 10);
                return base.ConvertToPollItemValues(values);
            }
            catch(Exception ex) {
                TDInternalLogger.GetLogger().LogException(new Exception("TDOpcV3PollProcess.Poll()", ex));
                return new CommonDataContract.PollItemValue[0];
            }
        }
        
    }
}
