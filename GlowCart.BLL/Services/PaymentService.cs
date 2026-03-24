using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GlowCart.BLL.Services
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ProcessPaymentAsync(PaymentDetails paymentDetails)
        {
            string apiUrl = "https://api.correctpaymentgateway.com/process";
            var response = await _httpClient.PostAsJsonAsync(apiUrl, paymentDetails);
            return response.IsSuccessStatusCode;
        }
    }

    public class PaymentDetails
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
    }
}