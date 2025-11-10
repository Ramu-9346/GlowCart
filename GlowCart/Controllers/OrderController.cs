using GlowCart.BLL;
using GlowCart.BLL.Services;
using GlowCart.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlowCart.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;

        public OrderController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            _orderService = new OrderService(conn);
            _cartService = new CartService(conn);
        }

        // ==============================
        // 🔹 GET: Checkout Page
        // ==============================
        [HttpGet]
        public IActionResult Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // Redirect to login if session expired
                return RedirectToAction("Auth", "Account");
            }

            // ✅ Load cart items for current user
            var cartItems = _cartService.GetCartItems(userId.Value);

            if (cartItems == null || !cartItems.Any())
            {
                TempData["Message"] = "⚠️ Your cart is empty. Please add items before checkout.";
                return RedirectToAction("ViewCart", "Cart");
            }

            // ✅ Pass cart items to Checkout View
            return View(cartItems);
        }

        // ==============================
        // 🔹 POST: Place Order (AJAX)
        // ==============================
        [HttpPost]
        public JsonResult PlaceOrder(string shippingAddress)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Json(new { success = false, message = "Please log in first." });

            var result = _orderService.PlaceOrder(userId.Value, shippingAddress);

            if (result.Success)
            {
                // Optionally clear cart after successful order
                _cartService.ClearCart(userId.Value);

                return Json(new
                {
                    success = true,
                    message = $"🎉 Order #{result.OrderId} placed successfully! Total: ₹{result.Total}"
                });
            }
            else
            {
                return Json(new { success = false, message = "⚠️ Order failed. Please try again." });
            }
        }
    }
}
