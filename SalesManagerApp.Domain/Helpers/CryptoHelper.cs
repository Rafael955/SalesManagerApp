using System.Security.Cryptography;
using System.Text;

namespace SalesManagerApp.Domain.Helpers
{
    /// <summary>
    /// Classe auxiliar para criptografia
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// Método para retornar um valor criptografado
        /// com algoritmo SHA256
        /// </summary>
        /// <param name="value">Texto a ser criptografado</param>
        /// <returns>Hash SHA256 em formato hexadecimal</returns>
        public static string GetSHA256(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                    builder.Append(b.ToString("x2")); // Converte cada byte para hexadecimal

                return builder.ToString();
            }
        }
    }
}
