using GlowCart.BLL;
using GlowCart.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlowCart.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _service;

        public CartController(IConfiguration configuration)
        {
            string conn = configuration.GetConnectionString("DefaultConnection");
            _service = new CartService(conn);
        }

        [HttpPost]
        public JsonResult AddToCart(int productId, int quantity)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            bool result = _service.AddToCart(userId, productId, quantity);
            return Json(new { success = result });
        }

        [HttpGet]
        public IActionResult ViewCart()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var cartItems = _service.GetCartItems(userId);
            return View(cartItems);
        }

        [HttpPost]
        public JsonResult RemoveItem(int cartId)
        {
            bool result = _service.RemoveCartItem(cartId);
            return Json(new { success = result });
        }

        [HttpPost]
        public JsonResult ClearCart()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            bool result = _service.ClearCart(userId);
            return Json(new { success = result });
        }
    }
}
