using System;
using PaymentProcessing.Models;
using PaymentProcessing.PaymentReceivers;
using PaymentProcessing.PaymentReceivers.PaymentHandlers;

namespace PaymentProcessing
{
    internal class Program
    {
        private static void Main()
        {
            var order = new Order();
            order.AddLineItem(new LineItem("GUID 1", "Product Name One", 499), 2);
            order.AddLineItem(new LineItem("GUID 2", "Product Name Two", 799), 1);

            order.AddPayment(new Payment
            {
                PaymentProvider = PaymentProvider.Paypal,
                Amount = 1000
            });

            order.AddPayment(new Payment
            {
                PaymentProvider = PaymentProvider.Invoice,
                Amount = 797
            });

            Console.WriteLine($"Amount Due:      \t {order.AmountDue}");
            Console.WriteLine($"Shipping Status: \t {order.ShippingStatus}");


            //var handler = new PaymentHandler(
            //    new PaypalHandler(),
            //    new InvoiceHandler(),
            //    new CreditCardHandler()
            //);
            var handler = new PaymentHandler()
                .SetNext(new PaypalHandler())
                .SetNext(new InvoiceHandler())
                .SetNext(new CreditCardHandler());
            handler.Handle(order);

            Console.WriteLine($"Amount Due:      \t {order.AmountDue}");
            Console.WriteLine($"Shipping Status: \t {order.ShippingStatus}");
        }
    }
}
