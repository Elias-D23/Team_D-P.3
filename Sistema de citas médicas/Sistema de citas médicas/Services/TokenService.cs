using System;
using System.Security.Cryptography;
using System.Text;

namespace Sistema_de_citas_médicas_.Services
{
    public class TokenService
    {
        public static string GenerarTokenValidacion(string correo)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(correo + DateTime.UtcNow));
                return Convert.ToBase64String(hash).Replace("/", "").Replace("+", "").Substring(0, 20);
            }
        }
    }
}
