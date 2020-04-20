using System;

namespace PaymentProcessing.Exceptions
{
    [Serializable]
    public class InsufficientPaymentException : ArgumentException
    {
    }
}
