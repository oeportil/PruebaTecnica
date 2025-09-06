using Dapper;
using Prueba_tecnica.Models;
using Prueba_tecnica.Repository.Interface;
using Prueba_tecnica.Util;

namespace Prueba_tecnica.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DapperContext _context;
        private readonly IConfiguration configuration;
        public UserRepository(DapperContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public async Task<User> CreateUser(User user)
        {
            //crear la sentencia sql
            var sqlfind = "SELECT * FROM users WHERE telefono = @Telefono OR email = @Email";
            using var connection = _context.CreateConnection();

            //buscar el usuario por su telefono y email
            var findUser = await connection.QueryFirstOrDefaultAsync<User>(sqlfind, new { Telefono = user.Telefono, Email = user.Email });
            if(findUser is not null) throw new InvalidOperationException($"Usuario con telefono {user.Telefono} o email {user.Email} ya existe");

            //hashear la contraseña
            string hashed = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashed;
            var sql = @"INSERT INTO users 
                        (nombres, apellidos, fecha_nacimiento, direccion, password, telefono, email, estado) 
                        VALUES (@Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Password, @Telefono, @Email, @Estado) RETURNING *";
            
            return await connection.QuerySingleAsync<User>(sql, user);
        }

        public async Task<User> DeleteUser(int id)
        {
            using var connection = _context.CreateConnection();
            //buscar el usuario a eliminar
            var findUser = await UtilFunctions.FindUser(id, connection)
                ?? throw new KeyNotFoundException($"Usuario con id {id} no encontrado");
        
            
            //eliminar usuario
            var sql = "DELETE FROM users WHERE id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });

            //retornar el usuario eliminado
            return findUser;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var sql = "SELECT * FROM users";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User> GetUser(int id)
        {
            using var connection = _context.CreateConnection();
            //buscar el usuario a eliminar
            var findUser = await UtilFunctions.FindUser(id, connection)
                ?? throw new KeyNotFoundException($"Usuario con id {id} no encontrado");
            return findUser;
        }

        public async Task<string> Login(string telefono, string password)
        {
            //crear la sentencia sql
            var sql = "SELECT * FROM users WHERE telefono = @Telefono";
            using var connection = _context.CreateConnection();

            //buscar el usuario por su telefono
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Telefono = telefono })
                ?? throw new KeyNotFoundException($"Usuario con telefono {telefono} no encontrado");

            //verificar la contraseña
            bool verified = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!verified) throw new UnauthorizedAccessException("Contraseña y/o telefono Incorrecto");

            //generar el token
            var token = UtilFunctions.GenerateJWT(user, configuration["Jwt:key"]!);

            //retornar el token
            return token;
        }

        public async Task<User> UpdateUser(User user, int id)
        {
            using var connection = _context.CreateConnection();
            //buscar el usuario a editar
            var findUser = await UtilFunctions.FindUser(id, connection)
                ?? throw new KeyNotFoundException($"Usuario con id {id} no encontrado");

            //Validar si la contraseña ha cambiado
            var password = user.Password != findUser.Password ? BCrypt.Net.BCrypt.HashPassword(user.Password)  :
               user.Password;
            
            var sqlUpdate = @"UPDATE users 
                      SET nombres = @Nombres, 
                          apellidos = @Apellidos, 
                          fecha_nacimiento = @FechaNacimiento, 
                          direccion = @Direccion, 
                          password = @Password, 
                          telefono = @Telefono, 
                          email = @Email, 
                          estado = @Estado 
                      WHERE id = @Id
                      RETURNING *";
            var updatedUser = await connection.QueryFirstOrDefaultAsync<User>(
               sqlUpdate,
               new
               {
                   user.Nombres,
                   user.Apellidos,
                   user.FechaNacimiento,
                   user.Direccion,
                   password,
                   user.Telefono,
                   user.Email,
                   user.Estado,
                   Id = id
               }
             );

            return updatedUser!;
        }
    }
}
