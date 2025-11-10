using GlowCart.Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Data;

namespace GlowCart.DAL.DAO
{
    public class UserDAO
    {
        private readonly string _connectionString;

        public UserDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 🔹 Register User
        public int RegisterUser(Registration model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", model.FullName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@Phone", model.Phone);
                    cmd.Parameters.AddWithValue("@Address", model.Address);

                    // 🟢 Capture RETURN VALUE from SQL procedure
                    SqlParameter returnParam = new SqlParameter("@ReturnVal", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(returnParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    // 🟢 Return procedure result
                    return Convert.ToInt32(returnParam.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RegisterUser: " + ex.Message);
                return 0;
            }
        }


        // 🔹 Validate Login
        public bool ValidateLogin(Login model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_LoginUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Password", model.Password);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.HasRows;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public int GetUserIdByEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT UserId FROM Users WHERE Email = @Email", con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                con.Open();

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

    }
}
