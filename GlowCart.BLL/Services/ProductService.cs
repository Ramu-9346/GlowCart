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

        public bool AddProduct(Product product)
        {
            return _productDAO.AddProduct(product);
        }

        public bool UpdateProduct(Product product)
        {
            return _productDAO.UpdateProduct(product);
        }

        public bool DeleteProduct(int id)
        {
            return _productDAO.DeleteProduct(id);
        }

        public Product GetProductDetails(int id)
        {
            return _productDAO.GetProductDetails(id);
        }
    }
}
