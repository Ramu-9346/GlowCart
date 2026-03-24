using System;

namespace GlowCart.BLL.Services
{
    public class PaymentService
    {
        public void ProcessPayment()
        {
            var paymentMethod = GetPaymentMethod();
            if (paymentMethod != null && paymentMethod.IsEnabled)
            {
                // Process payment
            }
            else
            {
                throw new InvalidOperationException("Payment method is not initialized or not enabled.");
            }
        }

        private PaymentMethod GetPaymentMethod()
        {
            // Logic to retrieve payment method
            return new PaymentMethod(); // Example return
        }
    }

    public class PaymentMethod
    {
        public bool IsEnabled { get; set; }
    }
}