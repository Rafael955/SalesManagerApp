using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SalesManagerApp.Configurations
{
    public static class AuthConfiguration
    {
        public static void AddAuthConfiguration(this IServiceCollection services)
        {
            var jwtSecret = "48B2E7A4-B89D-40AA-BB1C-73EC383FF8AA";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            //Criando a configuração para autenticação com JWT - JSON WEB TOKEN
            services.AddAuthentication(
                auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(
                    bearer =>
                    {
                        bearer.RequireHttpsMetadata = false;
                        bearer.SaveToken = true;
                        bearer.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = signingKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            // ajustar a janela de tolerância se necessário
                            ClockSkew = TimeSpan.FromMinutes(5)
                        };
                    });
        }
    }
}
