using GlowCart.BLL.Services;
using GlowCart.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Security.Cryptography.Xml;
using static System.Net.Mime.MediaTypeNames;

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
        //[HttpPost]
        //public JsonResult SaveProductWithImage(Product product, IFormFile? ProductImage)
        //{
        //    string imageFileName = product.ImageUrl ?? "noimage.png";

        //    try
        //    {
        //        if (ProductImage != null && ProductImage.Length > 0)
        //        {
        //            // ✅ Ensure Images folder exists
        //            string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        //            if (!Directory.Exists(imageFolder))
        //                Directory.CreateDirectory(imageFolder);

        //            // ✅ Unique file name
        //            imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProductImage.FileName);
        //            string filePath = Path.Combine(imageFolder, imageFileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                ProductImage.CopyTo(stream);
        //            }

        //            product.ImageUrl = imageFileName;
        //        }

        //        var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));

        //        bool success;
        //        string message;

        //        if (product.ProductId == 0)
        //        {
        //            success = productService.AddProduct(product);
        //            message = success ? "✅ Product added successfully!" : "❌ Failed to add product.";
        //        }
        //        else
        //        {
        //            success = productService.UpdateProduct(product);
        //            message = success ? "✅ Product updated successfully!" : "❌ Failed to update product.";
        //        }

        //        return Json(new { success, message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = $"⚠️ Error: {ex.Message}" });
        //    }
        //}

        [HttpGet]
        public JsonResult GetProduct(int id)
        {
            var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));
            var product = productService.GetProductDetails(id);
            if (product == null) return Json(new { success = false });
            return Json(new { success = true, product });
        }

        [HttpPost]
        public async Task<JsonResult> SaveProductWithImage()
        {
            // This method reads form fields + file (multipart via AJAX FormData)
            var form = Request.Form;
            int.TryParse(form["ProductId"].FirstOrDefault() ?? "0", out int productId);

            var product = new Product
            {
                ProductId = productId,
                ProductName = form["ProductName"],
                Brand = form["Brand"],
                Description = form["Description"],
                Price = decimal.TryParse(form["Price"], out var p) ? p : 0,
                IsAvailable = (form["IsAvailable"].FirstOrDefault() ?? "false") == "true"
            };

            // handle uploaded file
            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                if (file != null && file.Length > 0)
                {
                    // generate a safe unique filename
                    var fileName = Path.GetFileName(file.FileName);
                    var unique = $"{Guid.NewGuid():N}_{fileName}";
                    var env = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                    var savePath = Path.Combine(env.WebRootPath, "images", unique);

                    // ensure images folder exists
                    var dir = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // store the filename only in DB column ImageUrl
                    product.ImageUrl = unique;
                }
            }
            else
            {
                // If no uploaded file and this is update, keep existing ImageUrl (fetch from DB)
                if (productId > 0)
                {
                    var svc = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));
                    var existing = svc.GetProductDetails(productId);
                    if (existing != null) product.ImageUrl = existing.ImageUrl;
                }
                else
                {
                    product.ImageUrl = "noimage.png"; // fallback filename — put a noimage.png inside wwwroot/images
                }
            }

            var productService = new ProductService(HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"));
            bool success;
            if (product.ProductId == 0)
                success = productService.AddProduct(product);
            else
                success = productService.UpdateProduct(product);

            var message = success ? "Saved successfully" : "Save failed";
            return Json(new { success, message });
        }
       



    }
}
