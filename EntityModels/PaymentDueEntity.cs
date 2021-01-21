using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.EntityModels
{
    public class PaymentDueEntity
    {
        public string CustomerId { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DueDate { get; set; }
    }
}
