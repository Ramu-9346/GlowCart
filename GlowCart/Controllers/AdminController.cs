using GlowCart.BLL.Services;
using GlowCart.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlowCart.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        // ✅ Only one constructor
        public AdminController(IConfiguration config)
        {
            _adminService = new AdminService(config.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Auth", "Account");

            return View();
        }

        public IActionResult _Overview()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetOverviewStats()
        {
            var stats = _adminService.GetDashboardStats();
            return Json(new
            {
                totalProducts = stats.TotalProducts,
                totalOrders = stats.TotalOrders,
                totalUsers = stats.TotalUsers,
                totalSales = stats.TotalSales
            });
        }
        

[HttpGet]
    public IActionResult _Products()
    {
        var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));
        var products = productService.GetAllProducts();
        return PartialView(products);
    }

    [HttpPost]
    public JsonResult SaveProduct(Product product)
    {
        var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));

        bool success;
        string message;

        if (product.ProductId == 0)
        {
            success = productService.AddProduct(product);
            message = success ? "Product added successfully!" : "Failed to add product.";
        }
        else
        {
            success = productService.UpdateProduct(product);
            message = success ? "Product updated successfully!" : "Failed to update product.";
        }

        return Json(new { success, message });
    }

    [HttpPost]
    public JsonResult DeleteProduct(int id)
    {
        var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));
        bool success = productService.DeleteProduct(id);
        string message = success ? "Product deleted successfully!" : "Failed to delete product.";
        return Json(new { success, message });
    }

}
}
