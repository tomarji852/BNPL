using BNPL.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Services
{
    public interface IInventryService
    {
        List<Item> LoadInventry();
        List<Item> GetAllItems();
        void AddAllItems(List<Item> items);
        void ReduceItem(Item item);
        int GetNumberOfRemainingItemsCount();
    }
}
