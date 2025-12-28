using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesManagerApp.Domain.Helpers
{
    public class JwtTokenHelper
    {
        /// <summary>
        /// Método para gerar um TOKEN JWT
        /// </summary>
        public static string GenerateToken(string email, string perfil)
        {
            //Chave secreta utilizada para assinar o TOKEN
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("48B2E7A4-B89D-40AA-BB1C-73EC383FF8AA"));

            //Criptografar a assinatura do token
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Informações do usuário do token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email),  //nome do usuário autenticado
                new Claim(ClaimTypes.Role, perfil)  //perfil do usuário autenticado
            };

            //Criando o TOKEN JWT
            var token = new JwtSecurityToken(
                    claims: claims, //informações do usuário do token
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    signingCredentials: credentials
                );

            //retornando o TOKEN
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
