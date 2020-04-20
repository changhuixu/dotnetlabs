using PaymentProcessing.Models;

namespace PaymentProcessing.PaymentProcessors
{
    public interface IPaymentProcessor
    {
        void Finalize(Order order);
    }
}
