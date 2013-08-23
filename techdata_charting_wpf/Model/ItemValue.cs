using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    public class ItemValue {
        internal string ItemID { get; set; }
        internal object Value { get; set; }
        internal DateTime Timestamp { get; set; }
    }
}
