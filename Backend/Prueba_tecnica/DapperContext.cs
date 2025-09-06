using Npgsql;
using System.Data;

namespace Prueba_tecnica
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {          
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ConnectionPostgresql");
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
