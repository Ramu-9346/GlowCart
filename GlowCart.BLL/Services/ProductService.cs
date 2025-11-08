using GlowCart.DAL.DAO;
using GlowCart.Entities.Models;

namespace GlowCart.BLL.Services
{
    public class ProductService
    {
        private readonly ProductDAO _productDAO;

        public ProductService(string connectionString)
        {
            _productDAO = new ProductDAO();
        }

        public List<Product> GetAllProducts()
        {
            return _productDAO.GetAllProducts();
        }

        public Product GetProductDetails(int id)
        {
            return _productDAO.GetProductDetails(id);
        }
    }
}
