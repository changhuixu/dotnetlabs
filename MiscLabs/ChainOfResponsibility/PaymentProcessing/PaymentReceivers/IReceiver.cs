namespace PaymentProcessing.PaymentReceivers
{
    public interface IReceiver<in T> where T : class
    {
        void Handle(T request);
    }
}
