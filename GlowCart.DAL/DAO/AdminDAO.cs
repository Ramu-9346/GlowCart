using System.Data;
using Microsoft.Data.SqlClient;

namespace GlowCart.DAL.DAO
{
    public class AdminDAO
    {
        private readonly string _connectionString;

        public AdminDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 🔹 Fetch overall statistics for dashboard
        public (int TotalProducts, int TotalOrders, int TotalUsers, decimal TotalSales) GetDashboardStats()
        {
            int totalProducts = 0, totalOrders = 0, totalUsers = 0;
            decimal totalSales = 0;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDashboardStats", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    totalProducts = Convert.ToInt32(dr["TotalProducts"]);
                    totalOrders = Convert.ToInt32(dr["TotalOrders"]);
                    totalUsers = Convert.ToInt32(dr["TotalUsers"]);
                    totalSales = Convert.ToDecimal(dr["TotalSales"]);
                }
            }

            return (totalProducts, totalOrders, totalUsers, totalSales);
        }
    }
}
