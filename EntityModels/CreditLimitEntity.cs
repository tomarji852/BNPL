using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.EntityModels
{
    public class CreditLimitEntity
    {
        public string Customerid { get; set; }
        public string CustomerName { get; set; }
        public Decimal TotalCreditLimit { get; set; }
        public Decimal UsedCreditLimit { get; set; }        

        public Decimal getAvailableCreditLimit()
        {
            return TotalCreditLimit - UsedCreditLimit;
        }

        public void reduceCreditLimit(decimal amount)
        {
            UsedCreditLimit =+amount;
        }

    }
}
