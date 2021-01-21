using BNPL.EntityModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Services
{
    public class OrderService
    {
        private IInventryService _inventory;

        private List<OrderHistory> _orderHistories;

        public OrderService(IInventryService inventory, List<OrderHistory> orderHistories)
        {
            this._inventory = inventory;
            this._orderHistories = orderHistories;
        }

        public List<Item> LoadInputOrderItems()
        {
            List<Item> orderItems = new List<Item>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\inputOrderItems.json");
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                orderItems = JsonConvert.DeserializeObject<List<Item>>(json);
            }

            return orderItems;
        }

        public decimal BookOrder(List<Item> items, List<Item> orderedItems, string customerId)
        {
            decimal orderPrice = 0;
            if (_inventory.GetNumberOfRemainingItemsCount() > 0)
            {
                List<Item> availableItems =  _inventory.GetAllItems();
                foreach(var item in items)
                {
                    if (availableItems.Any(x => x.Name == item.Name))
                    {
                        _inventory.ReduceItem(item);
                        orderedItems.Add(item);
                        orderPrice += item.Price;
                    }
                    else
                    {
                        Console.WriteLine("Item is not present in inventry: " + item.Name);
                    }
                }
            }

            return orderPrice;
        }

        public bool ConfirmOrder(OrderHistory order, bool isPaymentSuccessfull)
        {
            bool isOrderConfirm = false;
            if (isPaymentSuccessfull)
            {
                //add item to order history table                        
                this.AddOrderDetail(order);
                isOrderConfirm = true;
            }
            else
            {
                // add back the items to inventry
                _inventory.AddAllItems(order.orderItems);
            }

            return isOrderConfirm;
        }

        public void AddOrderDetail(OrderHistory order)
        {
            _orderHistories.Add(order);
        }

        public void DeleteOrderDetail(OrderHistory order)
        {
            _orderHistories.Remove(order);
        }
    }
}
