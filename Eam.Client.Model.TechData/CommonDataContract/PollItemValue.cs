using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData.CommonDataContract {
    public class PollItemValue {
        public string ItemID { get; internal set; }
        public object Value { get; internal set; }
        public DateTime Timestamp { get; protected set; }
    }
}
