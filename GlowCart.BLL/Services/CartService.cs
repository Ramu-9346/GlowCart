using GlowCart.DAL;
using GlowCart.Entities.Models;

namespace GlowCart.BLL
{
    public class CartService
    {
        private readonly CartDAO _dao;

        public CartService(string connectionString)
        {
            _dao = new CartDAO(connectionString);
        }

        public bool AddToCart(int userId, int productId, int quantity) =>
            _dao.AddToCart(userId, productId, quantity);

        public List<Cart> GetCartItems(int userId) =>
            _dao.GetCartItems(userId);

        public bool RemoveCartItem(int cartId) =>
            _dao.RemoveCartItem(cartId);

        public bool ClearCart(int userId) =>
            _dao.ClearCart(userId);
    }
}
