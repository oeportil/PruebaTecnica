using Dapper;
using Microsoft.IdentityModel.Tokens;
using Prueba_tecnica.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prueba_tecnica.Util
{
    public class UtilFunctions
    {


        public static string GenerateJWT(User model, string key)
        {
            //create info for user token
            var userClaim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Token Details
            var jwtConfig = new JwtSecurityToken(
                claims: userClaim,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        public static async Task<User> FindUser(int id, IDbConnection connection)
        {
            //buscar el usuario 
            var user = await connection.QueryFirstOrDefaultAsync<User>(
               "SELECT * FROM users WHERE id = @Id",
               new { Id = id }
            ) ?? throw new KeyNotFoundException($"Usuario con id {id} no encontrado");

            return user;
        }
    }
}
