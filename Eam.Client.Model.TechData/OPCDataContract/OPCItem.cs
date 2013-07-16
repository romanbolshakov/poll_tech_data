using System;
using System.Collections.Generic;
using System.Text;

using Eam.Client.Model.TechData.CommonDataContract;

namespace Eam.Client.Model.TechData.OPCDataContract {
    public class OPCItem: PollItem {
        private OPCGroup _opcGroup;

        public string FullName {
            get { return base.ItemName; }
            set { base.ItemName = value; }
        }

        private OPCItemValue _value;
        public OPCItemValue Value {
            get { return _value; }
            set {
                if (_value != value) {
                    _value = value;
                    base.OnValueUpdated(_value);
                }
            }
        }

        private string _type;
        public string ItemType {
            get { return _type; }
            set { _type = value; }
        }

        public override string ToString() {
            return FullName;
        }

    }
}
