using Microsoft.Data.SqlClient;  // or System.Data.SqlClient
using System;
using System.Data;
using System.Threading.Tasks;

namespace EMS.Data.Contexts
{
    public class SqlServerContext
    {
        private readonly string _connectionString;

        public SqlServerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
