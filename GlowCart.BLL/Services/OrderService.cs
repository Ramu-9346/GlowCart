using GlowCart.DAL.DAO;

namespace GlowCart.BLL.Services
{
    public class OrderService
    {
        private readonly OrderDAO _orderDAO;

        public OrderService(string connectionString)
        {
            _orderDAO = new OrderDAO(connectionString);
        }

        public (bool Success, int OrderId, decimal Total) PlaceOrder(int userId, string address)
        {
            var result = _orderDAO.CreateOrder(userId, address);
            if (result.OrderId > 0)
                return (true, result.OrderId, result.TotalAmount);
            else
                return (false, 0, 0);
        }
    }
}
