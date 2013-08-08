using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData.CommonDataContract {
    public class SubPollItem : PollItem {

        private int _bitOffset;
        public int BitOffset {
            get { return _bitOffset; }
            set { _bitOffset = value; }
        }

        private int _bitCount;
        public int BitCount {
            get { return _bitCount; }
            set { _bitCount = value; }
        }
    }
}
