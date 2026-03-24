using System;
using GlowCart.Models;

namespace GlowCart.BLL.Services
{
    public class PaymentService
    {
        public string ProcessOrderPayment(Order order)
        {
            var paymentGateway = GetPaymentGateway();
            if (paymentGateway == null)
            {
                throw new InvalidOperationException("Payment gateway is not initialized.");
            }
            var transactionId = paymentGateway.ProcessPayment(order);
            return transactionId;
        }

        private IPaymentGateway GetPaymentGateway()
        {
            // Logic to retrieve and initialize the payment gateway
            return new PaymentGateway(); // Assuming PaymentGateway implements IPaymentGateway
        }
    }

    public interface IPaymentGateway
    {
        string ProcessPayment(Order order);
    }

    public class PaymentGateway : IPaymentGateway
    {
        public string ProcessPayment(Order order)
        {
            // Process payment logic
            return "transaction-id-123";
        }
    }

    public class Order
    {
        // Order properties
    }
}