using System;
using GlowCart.Models;

namespace GlowCart.BLL.Services
{
    public class PaymentService
    {
        private PaymentGateway paymentGateway;

        public PaymentService()
        {
            InitializePaymentGateway();
        }

        private void InitializePaymentGateway()
        {
            // Ensure the paymentGateway is initialized
            paymentGateway = new PaymentGateway
            {
                Url = "https://defaultgateway.com",
                ApiKey = "defaultApiKey",
                Secret = "defaultSecret"
            };
        }

        public string GetGatewayUrl()
        {
            if (paymentGateway == null)
            {
                throw new InvalidOperationException("Payment gateway is not initialized.");
            }
            return paymentGateway.Url;
        }

        public void ProcessPayment()
        {
            try
            {
                var gatewayUrl = GetGatewayUrl();
                // Proceed with payment processing using gatewayUrl
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error processing payment: {ex.Message}");
            }
        }
    }

    public class PaymentGateway
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
    }
}