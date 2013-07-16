using System;
using System.Collections.Generic;
using System.Text;

using Eam.Client.Model.TechData.CommonDataContract;

namespace Eam.Client.Model.TechData.OPCDataContract {
    public class OPCItem: PollItem {
        private OPCGroup _opcGroup;
        private string _name;
        public string ItemName {
            get { return _name; }
            set { _name = value; }
        }

        private string _fullName;
        public string FullName {
            get { return _fullName; }
            set { _fullName = value; }
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
            return _name;
        }

    }
}
