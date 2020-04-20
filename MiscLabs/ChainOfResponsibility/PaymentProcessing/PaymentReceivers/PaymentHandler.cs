using System;
using System.Collections.Generic;
using PaymentProcessing.Exceptions;
using PaymentProcessing.Models;

namespace PaymentProcessing.PaymentReceivers
{
    public class PaymentHandler
    {
        private readonly List<IReceiver<Order>> _receivers = new List<IReceiver<Order>>();

        public PaymentHandler(params IReceiver<Order>[] receivers)
        {
            _receivers.AddRange(receivers);
        }

        public void Handle(Order order)
        {
            foreach (var receiver in _receivers)
            {
                Console.WriteLine($"Payment Receiver: \t {receiver.GetType().Name}");

                if (order.AmountDue > 0)
                {
                    receiver.Handle(order);
                }
                else
                {
                    break;
                }
            }

            if (order.AmountDue > 0)
            {
                throw new InsufficientPaymentException();
            }

            order.ShippingStatus = ShippingStatus.ReadyForShipment;
        }

        public PaymentHandler SetNext(IReceiver<Order> next)
        {
            _receivers.Add(next);
            return this;
        }
    }
}
