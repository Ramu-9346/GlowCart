using System.Data;
using Microsoft.Data.SqlClient;
using GlowCart.Entities.Models;

namespace GlowCart.DAL
{
    public class CartDAO
    {
        private readonly string _connectionString;

        public CartDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddToCart(int userId, int productId, int quantity)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddToCart", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                // ✅ Create return value parameter
                SqlParameter returnParam = new SqlParameter("@ReturnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParam);

                con.Open();
                cmd.ExecuteNonQuery();

                int result = (int)returnParam.Value;
                Console.WriteLine($"Stored procedure returned: {result}");
                return result > 0;
            }
        }

        

        public List<Cart> GetCartItems(int userId)
        {
            List<Cart> carts = new List<Cart>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetCartItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    carts.Add(new Cart
                    {
                        CartId = Convert.ToInt32(dr["CartId"]),
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Total = Convert.ToDecimal(dr["Total"]),
                        ImageUrl = dr["ImageUrl"].ToString()
                    });
                }
            }
            return carts;
        }

        public bool RemoveCartItem(int cartId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_RemoveCartItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CartId", cartId);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ClearCart(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ClearCart", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
