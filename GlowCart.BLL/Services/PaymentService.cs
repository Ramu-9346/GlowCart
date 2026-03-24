using System;
using GlowCart.BLL.Factories;
using GlowCart.BLL.Models;

namespace GlowCart.BLL.Services
{
    public class PaymentService
    {
        private readonly IPaymentGatewayFactory paymentGatewayFactory;

        public PaymentService(IPaymentGatewayFactory paymentGatewayFactory)
        {
            this.paymentGatewayFactory = paymentGatewayFactory ?? throw new ArgumentNullException(nameof(paymentGatewayFactory));
        }

        public void ProcessOrderPayment(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            var gateway = paymentGatewayFactory.CreateGateway();
            if (gateway == null) throw new InvalidOperationException("Payment gateway could not be created.");

            if (gateway.IsActive)
            {
                gateway.ProcessPayment(order);
            }
            else
            {
                throw new InvalidOperationException("Payment gateway is not active.");
            }
        }
    }
}