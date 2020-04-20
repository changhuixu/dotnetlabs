using System.Linq;
using PaymentProcessing.Models;
using PaymentProcessing.PaymentProcessors;

namespace PaymentProcessing.PaymentReceivers.PaymentHandlers
{
    public class PaypalHandler : IReceiver<Order>
    {
        private readonly PaypalPaymentProcessor _paypalPaymentProcessor = new PaypalPaymentProcessor();

        public void Handle(Order order)
        {
            if (order.SelectedPayments.Any(x => x.PaymentProvider == PaymentProvider.Paypal))
            {
                _paypalPaymentProcessor.Finalize(order);
            }
        }
    }
}
