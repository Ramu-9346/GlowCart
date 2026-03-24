using System;
using GlowCart.Models;

namespace GlowCart.BLL.Services
{
    public class PaymentService
    {
        public void ProcessPayment()
        {
            var paymentMethod = GetPaymentMethod();
            if (paymentMethod != null && paymentMethod.IsActive)
            {
                // Process payment
            }
            else
            {
                throw new InvalidOperationException("Payment method is not initialized or inactive.");
            }
        }

        private PaymentMethod GetPaymentMethod()
        {
            // Logic to retrieve the payment method
            return new PaymentMethod { IsActive = true }; // Example implementation
        }
    }
}
