using Microsoft.AspNetCore.Mvc;
using GlowCart.BLL.Services;

namespace GlowCart.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _productService = new ProductService(connectionString);
        }

        public IActionResult GetProducts()
        {
            var products = _productService.GetAllProducts();
            return View("Product", products);
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetProductDetails(id);
            if (product == null)
                return Content("<div class='text-danger'>Product not found!</div>");

            return PartialView("_ProductDetailsPartial", product);
        }
    }
}
