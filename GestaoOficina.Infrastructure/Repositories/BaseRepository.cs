using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BaseRepository
    {
        string CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        protected async Task<IEnumerable<T>> ListarAsync<T>(string sqlCommand, object parms)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                return await conn.QueryAsync<T>(sqlCommand, parms);
            }
        }

        protected async Task<T> ObterAsync<T>(string sqlCommand, object? parms)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                return (await conn.QueryAsync<T>(sqlCommand, parms)).FirstOrDefault();
            }
        }

        protected async Task<int> ExecutarAsync(string sqlCommand, object? parms)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                return await conn.ExecuteAsync(sqlCommand, parms);
            }
        }


    }
}
