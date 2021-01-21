using BNPL.EntityModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BNPL.Services
{
    public class InventryService : IInventryService
    {
        private List<Item> items;

        public InventryService(List<Item> items)
        {
            this.items = items;
        }

        /// <summary>
        /// this method loads the inventry
        /// </summary>
        /// <returns>List<Item></returns>
        public List<Item> LoadInventry()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\inputInvenory.json");
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                items.AddRange(JsonConvert.DeserializeObject<List<Item>>(json));
            }

            return items;
        }

        /// <summary>
        /// This method removes the item form the inventory
        /// </summary>
        /// <param name="item"></param>
        public void ReduceItem(Item item)
        {
            this.items.Remove(items.FirstOrDefault(x => x.Name == item.Name));            
        }
      

        /// <summary>
        /// this method provide details of all items at anytime
        /// </summary>
        /// <returns>List<Item></returns>
        public List<Item> GetAllItems()
        {            
            return this.items;
        }

        /// <summary>
        /// this method is adding items to inventry back
        /// </summary>
        /// <param name="items"></param>
        public void AddAllItems(List<Item> unOrdereditems)
        {
            this.items.AddRange(unOrdereditems);
        }

        /// <summary>
        /// this method provide count of all remaining items at anytime
        /// </summary>
        /// <returns>integer count</returns>
        public int GetNumberOfRemainingItemsCount()
        {            
            return this.items.Count();
        }
    }
}
