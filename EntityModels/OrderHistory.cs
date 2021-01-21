using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.EntityModels
{
    public class OrderHistory
    {
        public string CustomerId { get; set; }
        public List<Item> orderItems { get; set; }
        public string PaymentTransactionRefNumber { get; set; }
        public decimal OrderPrice { get; set; }
        public PaymentType paymentType { get; set; }
    }

    public enum PaymentType
    {
        BNPL,
        Prepayment
    }
}
