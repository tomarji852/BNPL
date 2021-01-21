using BNPL.EntityModels;
using BNPL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Item> InventryItems = new List<Item>();
            List<CustomerEntity> customers = new List<CustomerEntity>();
            List<OrderHistory> orderHistories = new List<OrderHistory>();
            List<PaymentDueEntity> paymentDueCustomers = new List<PaymentDueEntity>();
            IInventryService inventry = new InventryService(InventryItems);
            OrderService orderService = new OrderService(inventry, orderHistories);
            CreditLimitService creditLimitService = new CreditLimitService();
            

            try
            {
                List<Item> items = inventry.LoadInventry();
                List<CreditLimitEntity> customerCreditLimits = creditLimitService.LoadCreditLimitData();
                PaymentService paymentService = new PaymentService(customerCreditLimits);

                string runningCondition = "1";
                while (runningCondition == "1")
                {
                    Console.WriteLine("Please enter the action you want to perform");
                    Console.WriteLine("Order Booking press: 1");
                    Console.WriteLine("Clearing dues press: 2");
                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.WriteLine("Please enter the customerId");
                        string customerId = Console.ReadLine();

                        if (!customers.Any(x => x.CustomerId == customerId))
                        {
                            Console.WriteLine("This is a new customer");
                            Console.WriteLine("please enter your name");
                            string customerName = Console.ReadLine();
                            Console.WriteLine("please enter your phone number");
                            string phoneNumber = Console.ReadLine();
                            customers.Add(new CustomerEntity { CustomerId = customerId,CustomerName = customerName,PhoneNumber = phoneNumber, isActive = true });
                        }
                            

                        bool isActiveCustomer = customers.FirstOrDefault(x => x.CustomerId == customerId).isActive;
                        List<Item> inputOrderItems = orderService.LoadInputOrderItems();
                        List<Item> orderedItems = new List<Item>(); 
                        
                        if(isActiveCustomer)
                            BookOrder(customerId, orderService, paymentService, paymentDueCustomers,
                                inputOrderItems, orderedItems);
                        else
                        {
                            Console.WriteLine("Order cant be placed ,This customer is blocked");
                        }
                    }
                    else if (input == "2")
                    {
                        Console.WriteLine("Please enter the customerId");
                        string customerId = Console.ReadLine();
                        if (customers.Any(x => x.CustomerId == customerId))
                        {
                            Console.WriteLine("Please enter the payment amount");
                            string strPaymentAmount = Console.ReadLine();
                            decimal paymentAmount = Convert.ToDecimal(strPaymentAmount);
                            ClearDues(paymentDueCustomers, customerId, paymentAmount);
                        }
                        else
                        {
                            Console.WriteLine("No customer found for given id");
                        }                                              
                    }

                    Console.WriteLine("you want to run again");
                    Console.WriteLine("Run for another process press:1");
                    Console.WriteLine("Exit process press:2");
                    runningCondition = Console.ReadLine();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }                      
        }

        static void BookOrder(string customerId, OrderService orderService, PaymentService paymentService,
            List<PaymentDueEntity> paymentDueCustomers, List<Item> inputOrderItems, List<Item> orderedItems)
        {
            try
            {                   
                decimal orderPrice = orderService.BookOrder(inputOrderItems, orderedItems, customerId);

                Console.WriteLine("Order price is :" + orderPrice.ToString());
                Console.WriteLine("Please press 1 for prepayment 2 for Pay later");
                string decision = Console.ReadLine();

                

                OrderHistory orderDetails = new OrderHistory()
                {
                    CustomerId = customerId,
                    orderItems = orderedItems,
                    OrderPrice = orderPrice,
                    PaymentTransactionRefNumber = "payment",
                    paymentType = PaymentType.Prepayment
                };
                bool isOrderConfirm = false;
                if (decision == "1")
                {
                    bool isSuccessfull = paymentService.GetPayment(orderPrice);
                    isOrderConfirm = orderService.ConfirmOrder(orderDetails, isSuccessfull);
                }
                else if (decision == "2")
                {
                    bool isSuccessfull = paymentService.GetPaymentByCreditLimit(orderPrice, customerId);
                    isOrderConfirm = orderService.ConfirmOrder(orderDetails, isSuccessfull);
                }
                else
                {
                    Console.WriteLine("You have entered wrong input for payment");
                }

                if (decision == "2" && isOrderConfirm)
                {
                    if(paymentDueCustomers.Any(x => x.CustomerId == orderDetails.CustomerId))
                    {
                        PaymentDueEntity customer = paymentDueCustomers.FirstOrDefault(x => x.CustomerId == orderDetails.CustomerId);
                        customer.DueAmount += orderPrice;
                    }
                    else
                    {
                        PaymentDueEntity customer = new PaymentDueEntity()
                        {
                            CustomerId = orderDetails.CustomerId,
                            DueAmount = orderPrice,
                            DueDate = DateTime.Now.AddDays(30)
                        };
                        paymentDueCustomers.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void ClearDues(List<PaymentDueEntity> paymentDueCustomers, string customerId, decimal paymentAmount)
        {
            if (paymentDueCustomers.Any(x => x.CustomerId == customerId))
            {
                PaymentDueEntity customer = paymentDueCustomers.FirstOrDefault(x => x.CustomerId == customerId);
                if (paymentAmount > customer.DueAmount)
                {
                    customer.DueAmount = 0;
                    decimal extraAmount = paymentAmount - customer.DueAmount;
                    Console.WriteLine("You have entered extra amount of" + extraAmount.ToString());
                    Console.WriteLine("you will get the refund for the same");
                }
                else
                {
                    customer.DueAmount -= paymentAmount;
                }
                    
                
            }
        } 
    }
}
