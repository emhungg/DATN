using StudySystem.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Infrastructure.Extensions
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            string salt = AppSetting.Salt; // Lấy salt từ appsettings

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Kết hợp salt với mật khẩu trước khi mã hóa
                byte[] saltedPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];
                Buffer.BlockCopy(saltBytes, 0, saltedPasswordBytes, 0, saltBytes.Length);
                Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, saltBytes.Length, passwordBytes.Length);

                byte[] hashedBytes = sha256.ComputeHash(saltedPasswordBytes);
                string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(password);
            return string.Equals(hashedInputPassword, hashedPassword);
        }
    }
}
