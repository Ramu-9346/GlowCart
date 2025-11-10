using GlowCart.DAL.DAO;

namespace GlowCart.BLL.Services
{
    public class AdminService
    {
        private readonly AdminDAO _adminDAO;

        public AdminService(string connectionString)
        {
            _adminDAO = new AdminDAO(connectionString);
        }

        public (int TotalProducts, int TotalOrders, int TotalUsers, decimal TotalSales) GetDashboardStats()
        {
            return _adminDAO.GetDashboardStats();
        }
    }
}
