using System.Linq;
using PaymentProcessing.Models;
using PaymentProcessing.PaymentProcessors;

namespace PaymentProcessing.PaymentReceivers.PaymentHandlers
{
    public class CreditCardHandler : IReceiver<Order>
    {
        private readonly CreditCardPaymentProcessor _creditCardPaymentProcessor = new CreditCardPaymentProcessor();

        public void Handle(Order order)
        {
            if (order.SelectedPayments.Any(x => x.PaymentProvider == PaymentProvider.CreditCard))
            {
                _creditCardPaymentProcessor.Finalize(order);
            }
        }
    }
}
