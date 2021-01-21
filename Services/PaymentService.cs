using BNPL.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Services
{
    public class PaymentService
    {
        private List<CreditLimitEntity> _creditLimitEntities;

        public PaymentService(List<CreditLimitEntity> creditLimitEntities)
        {
            this._creditLimitEntities = creditLimitEntities;
        }
        public bool GetPayment(decimal orderPrice)
        {
            bool isPaymentSuccessfull = true;

            return isPaymentSuccessfull;
        }

        public bool GetPaymentByCreditLimit(decimal orderPrice, string customerId)
        {
            CreditLimitEntity creditLimitEntity = _creditLimitEntities.FirstOrDefault(x => x.Customerid == customerId);
            if (creditLimitEntity.getAvailableCreditLimit()>= orderPrice)
            {
                creditLimitEntity.UsedCreditLimit += orderPrice;
                return true;
            }
            else
            {
                Console.WriteLine("you have not enough credit limit");
                return false;
            }
        }
    }
}
