using GlowCart.Entities.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration; // ✅ For reading from appsettings.json

namespace GlowCart.DAL.DAO
{
    public class ProductDAO
    {
        private readonly string _connectionString;

        // ✅ Constructor automatically reads connection string from appsettings.json
        public ProductDAO()
        {
            // Build configuration to read from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ✅ Get all products
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetProducts", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var product = new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Description = dr["Description"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        ImageUrl = dr["ImageUrl"].ToString(),
                        Brand = dr["Brand"] != DBNull.Value ? dr["Brand"].ToString() : "N/A",
                        IsAvailable = dr["IsAvailable"] != DBNull.Value && Convert.ToBoolean(dr["IsAvailable"])
                    };
                    products.Add(product);
                }
            }

            return products;
        }

        // ✅ Get single product details
        public Product GetProductDetails(int productId)
        {
            Product product = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetProductDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", productId);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    product = new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Description = dr["Description"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        ImageUrl = dr["ImageUrl"].ToString(),
                        Brand = dr["Brand"] != DBNull.Value ? dr["Brand"].ToString() : "N/A",
                        IsAvailable = dr["IsAvailable"] != DBNull.Value && Convert.ToBoolean(dr["IsAvailable"])
                    };
                }
            }

            return product;
        }
        // ✅ Add a new product
        public bool AddProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Description", product.Description ?? "");
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl ?? "noimage.png");
                cmd.Parameters.AddWithValue("@Brand", product.Brand ?? "N/A");
                cmd.Parameters.AddWithValue("@IsAvailable", product.IsAvailable);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ✅ Update an existing product
        public bool UpdateProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductID", product.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Description", product.Description ?? "");
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl ?? "noimage.png");
                cmd.Parameters.AddWithValue("@Brand", product.Brand ?? "N/A");
                cmd.Parameters.AddWithValue("@IsAvailable", product.IsAvailable);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ✅ Delete a product
        public bool DeleteProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", productId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

    }
}
