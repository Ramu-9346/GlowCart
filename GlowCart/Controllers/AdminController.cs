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
     [HttpGet]
     public IActionResult _AddEditProduct(int? id)
     {
         var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));
         Product product = id.HasValue ? productService.GetProductDetails(id.Value) : new Product();
         return PartialView(product);
     }
        [HttpPost]
        public JsonResult SaveProductWithImage(Product product, IFormFile? ProductImage)
        {
            string imageFileName = product.ImageUrl ?? "noimage.png";

            try
            {
                if (ProductImage != null && ProductImage.Length > 0)
                {
                    // ✅ Ensure Images folder exists
                    string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imageFolder))
                        Directory.CreateDirectory(imageFolder);

                    // ✅ Unique file name
                    imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProductImage.FileName);
                    string filePath = Path.Combine(imageFolder, imageFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ProductImage.CopyTo(stream);
                    }

                    product.ImageUrl = imageFileName;
                }

                var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));

                bool success;
                string message;

                if (product.ProductId == 0)
                {
                    success = productService.AddProduct(product);
                    message = success ? "✅ Product added successfully!" : "❌ Failed to add product.";
                }
                else
                {
                    success = productService.UpdateProduct(product);
                    message = success ? "✅ Product updated successfully!" : "❌ Failed to update product.";
                }

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"⚠️ Error: {ex.Message}" });
            }
        }





    }
}
