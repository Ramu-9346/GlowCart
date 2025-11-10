using GlowCart.DAL.DAO;
using GlowCart.Entities.Models;

namespace GlowCart.BLL.Services
{
    public class UserService
    {
        private readonly UserDAO _userDAO;

        public UserService(string connectionString)
        {
            _userDAO = new UserDAO(connectionString);
        }

        // 🔹 Register user via DAO
        public (bool success, string message) RegisterUser(Registration model)
        {
            int result = _userDAO.RegisterUser(model);

            return result switch
            {
                1 => (true, "✅ Registration successful!"),
                -1 => (false, "⚠️ Email already exists."),
                _ => (false, "❌ Registration failed. Please try again.")
            };
        }

        // 🔹 Validate login via DAO
        public bool ValidateLogin(Login model)
        {
            return _userDAO.ValidateLogin(model);
        }
        public int GetUserIdByEmail(string email)
        {
            return _userDAO.GetUserIdByEmail(email);
        }
        public string GetUserRoleByEmail(string email)
        {
            return  _userDAO.GetUserRoleByEmail(email);
        }

    }
}
