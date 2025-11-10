using System.Data;
using Microsoft.Data.SqlClient;
using GlowCart.Entities.Models;

namespace GlowCart.DAL.DAO
{
    public class OrderDAO
    {
        private readonly string _connectionString;

        public OrderDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public (int OrderId, decimal TotalAmount) CreateOrder(int userId, string shippingAddress)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CreateOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ShippingAddress", shippingAddress);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (Convert.ToInt32(reader["OrderId"]), Convert.ToDecimal(reader["TotalAmount"]));
                    }
                }
            }

            return (0, 0);
        }
    }
}
