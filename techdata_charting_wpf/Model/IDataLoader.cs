using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    internal interface IDataLoader {
        /// <summary>
        /// Load techdate for time period
        /// </summary>
        /// <param name="itemName">item name</param>
        /// <param name="dateFrom">begin time period</param>
        /// <param name="dateTo">end of time period</param>
        /// <returns></returns>
        ItemValueCollection LoadItemValueCollection(
            string itemName, DateTime dateFrom, DateTime? dateTo);
        /// <summary>
        /// Load new techdata after concrete timestamp (for update data)
        /// </summary>
        /// <param name="_itemName"></param>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        ItemValueCollection LoadItemValueCollection(string itemName, DateTime lastTimestamp);
        /// <summary>
        /// Загружает все зарегистрированные в !БД! параметры (items)
        /// </summary>
        /// <returns></returns>
        ItemCollection LoadAllItemCollection();
        /// <summary>
        /// Загружает список параметров, котрые определен в коллекции с номером collectionID
        /// </summary>
        /// <param name="collectionID"></param>
        /// <returns></returns>
        ItemCollection LoadItemCollection(int collectionID);
        /// <summary>
        /// Возвращает последнее значение параметра на указанную дату и время
        /// Если дата и время не указаны (=null), то возвращается 
        /// последнее значение на текущую дату и время
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="customDateTime"></param>
        /// <returns></returns>
        ItemValue GetLastItemValue(string itemName, DateTime? customDateTime);
        /// <summary>
        /// Загружает Описание Коллекций контроллируемых параметров
        /// </summary>
        /// <returns></returns>
        List<CustomItemCollection> LoadCustomItemCollections();
    }
}
