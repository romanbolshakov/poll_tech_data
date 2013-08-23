using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    /// <summary>
    /// Коллекция контроллируемых параметров (сущность из БД)
    /// </summary>
    public class CustomItemCollection {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
