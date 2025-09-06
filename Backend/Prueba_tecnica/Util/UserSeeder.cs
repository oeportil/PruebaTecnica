using Dapper;
using Prueba_tecnica.Models;
using System.Data;

namespace Prueba_tecnica.Util
{

    public class UserSeeder
    {
        private readonly DapperContext context;

        public UserSeeder(DapperContext context)
        {
           this.context = context;
        }

        public async Task SeedDefaultUsers()
        {
            var _connection = context.CreateConnection();
            var count = await _connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Users");

            if (count > 0)
            {
                Console.WriteLine("Ya existen usuarios en la base de datos.");
                return;
            }

            var defaultUsers = new List<(string Nombres, string Apellidos, string Email, string Password, string telefono)>
        {
            ("Ana", "Gonzalez", "ana.gonzalez@example.com", "AnaPass123", "71234567"),
            ("Luis", "Ramirez", "luis.ramirez@example.com", "LuisPass456",  "77771111"),
            ("Maria", "Lopez", "maria.lopez@example.com", "MariaPass789",  "77770000"),
            ("Carlos", "Martinez", "carlos.martinez@example.com", "CarlosPass321", "77773241")
        };

            foreach (var u in defaultUsers)
            {
                // Hashear la contraseña
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(u.Password);

                var sqlInsert = @"
                INSERT INTO Users 
                (nombres, apellidos, fecha_nacimiento, direccion, password, telefono, email, estado, fechaCreacion)
                VALUES (@Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Password, @Telefono, @Email, 'A', CURRENT_TIMESTAMP);
            ";

                var newUser = new User
                {
                    Nombres = u.Nombres,
                    Apellidos = u.Apellidos,
                    FechaNacimiento = DateTime.UtcNow.AddYears(-25), // Ejemplo: 25 años atrás
                    Direccion = "Direccion Generica",
                    Password = hashedPassword,
                    Telefono = u.telefono,
                    Email = u.Email,
                };

                await _connection.ExecuteAsync(sqlInsert, newUser);

                Console.WriteLine($"Usuario generado: {u.Email} - Contraseña: {u.Password}");
            }
        }
    }
}
