using GlowCart.BLL.Services;
using GlowCart.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlowCart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(IConfiguration config)
        {
            _userService = new UserService(config.GetConnectionString("DefaultConnection"));
        }

        // ==============================
        // 🔹 Show Login/Register Page
        // ==============================
        public IActionResult Auth()
        {
            // If already logged in, redirect to Products page
            if (HttpContext.Session.GetString("UserEmail") != null)
            {
                return RedirectToAction("GetProducts", "Product");
            }

            return View();
        }

        // ==============================
        // 🔹 Register via AJAX
        // ==============================
        [HttpPost]
        public JsonResult Register(Registration model)
        {
            var result = _userService.RegisterUser(model);
            return Json(new { success = result.success, message = result.message });
        }

        // ==============================
        // 🔹 Login via AJAX
        // ==============================

        [HttpPost]
        public JsonResult Login(Login model)
        {
            bool isValid = _userService.ValidateLogin(model);

            if (isValid)
            {
                int userId = _userService.GetUserIdByEmail(model.Email);
                string role = _userService.GetUserRoleByEmail(model.Email);

                // ✅ Store in Session
                HttpContext.Session.SetString("UserEmail", model.Email);
                HttpContext.Session.SetInt32("UserId", userId);
                HttpContext.Session.SetString("UserRole", role);

                //  Redirect based on Role
                string? redirectUrl = role == "Admin"
                    ? Url.Action("Dashboard", "Admin")
                    : Url.Action("Index", "Home");

                return Json(new { success = true, redirectUrl });
            }
            else
            {
                return Json(new { success = false, message = "Invalid email or password." });
            }
        }



        // ==============================
        // 🔹 Logout
        // ==============================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); //  Destroy all session data
            return RedirectToAction("Auth", "Account");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return PartialView("_ForgotPasswordPartial");
        }

        [HttpPost]
        public JsonResult ForgotPassword(string Email)
        {
            // Simulate DB lookup (you can add real logic later)
            bool userExists = true; // Replace with check from DB

            if (userExists)
            {
                // TODO: Send reset email logic
                return Json(new { success = true, message = "Password reset link sent to your email!" });
            }
            else
            {
                return Json(new { success = false, message = "Email not found. Please try again." });
            }
        }

    }
}
