using Microsoft.AspNetCore.Mvc;

namespace GlowCart.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // ✅ Check if user is logged in
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Auth", "Account");
            }

            ViewBag.UserName = userEmail.Split('@')[0]; // Display name from email
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
