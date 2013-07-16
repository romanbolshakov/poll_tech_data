using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData.OPCDataContract {
    public class OPCItemValue: CommonDataContract.PollItemValue {
        public int ClientHandle { get; internal set; }
        public int Error { get; internal set; }
        public int MaxAge { get; internal set; }
        public int ServerHandle { get; internal set; }
        public string Quality { get; internal set; }
        public bool QualitySpecified { get; internal set; }
        public bool TimestampSpecified { get; internal set; }


        public OPCItemValue() {
        }

        public OPCItemValue(OPCDA.NET.ItemValue currentOPCItemValue) {
            FillData(currentOPCItemValue);
        }

        public void FillData(OPCDA.NET.ItemValue opcValue) {
            ClientHandle = opcValue.ClientHandle;
            Error = opcValue.Error;
            ItemID = opcValue.ItemID;
            MaxAge = opcValue.MaxAge;
            ServerHandle = opcValue.ServerHandle;
            Quality = opcValue.Quality.QualityField.ToString();
            QualitySpecified = opcValue.QualitySpecified;
            if (opcValue.TimestampSpecified)
                Timestamp = opcValue.Timestamp;
            else
                Timestamp = DateTime.Now;
            TimestampSpecified = opcValue.TimestampSpecified;
            Value = opcValue.Value;
        }
    }
}
