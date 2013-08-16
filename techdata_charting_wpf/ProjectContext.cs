using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf {
    internal class ProjectContext {
        private static ProjectContext _instance;
        internal static ProjectContext Current {
            get { return _instance; }
        }
        internal static void Create(Model.IDataLoader dataLoader) {
            _instance = new ProjectContext(dataLoader);
        }

        internal Model.IDataLoader DataLoader { get; private set; }

        private ProjectContext(Model.IDataLoader dataLoader) {
            DataLoader = dataLoader;
        }
    
    }
}
