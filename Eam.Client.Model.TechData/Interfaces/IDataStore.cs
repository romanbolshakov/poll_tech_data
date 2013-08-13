using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData.Interfaces {
    public interface IDataStore {
        void SaveValues(List<CommonDataContract.PollItem> updatedPollItems);
    }
}
