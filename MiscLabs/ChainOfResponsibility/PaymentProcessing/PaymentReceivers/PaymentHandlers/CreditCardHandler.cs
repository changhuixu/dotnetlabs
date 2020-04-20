using System.Linq;
using PaymentProcessing.Models;

namespace PaymentProcessing.PaymentReceivers.PaymentHandlers
{
    public class CreditCardHandler : IReceiver<Order>
    {
        public void Handle(Order order)
        {
            // Invoke the Stripe API
            var payment = order.SelectedPayments.FirstOrDefault(x => x.PaymentProvider == PaymentProvider.CreditCard);

            if (payment == null) return;

            order.FinalizePayment(payment);
        }
    }
}
