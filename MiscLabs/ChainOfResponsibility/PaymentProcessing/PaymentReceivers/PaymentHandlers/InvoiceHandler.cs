using System.Linq;
using PaymentProcessing.Models;
using PaymentProcessing.PaymentProcessors;

namespace PaymentProcessing.PaymentReceivers.PaymentHandlers
{
    public class InvoiceHandler : IReceiver<Order>
    {
        private readonly InvoicePaymentProcessor _invoicePaymentProcessor = new InvoicePaymentProcessor();

        public void Handle(Order order)
        {
            if (order.SelectedPayments.Any(x => x.PaymentProvider == PaymentProvider.Invoice))
            {
                _invoicePaymentProcessor.Finalize(order);
            }
        }
    }
}
