using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Reflection;
using LabCMS.TestReportDomain.Shared;
using System.Text;

namespace LabCMS.TestReportDomain.Server.Services
{
    public class DatabaseAccessService:IAsyncDisposable
    {
        private readonly IDbConnection _connection;
        public DatabaseAccessService(string connectionString)
        { _connection = new NpgsqlConnection(connectionString); }
        

        public async ValueTask<IEnumerable<dynamic>> GetAllAsync(string tableName)=>
            await _connection.QueryAsync($"SELECT * FROM {tableName};");
        
        public async ValueTask InsertAsync<TEntity>(TEntity entity,string tableName)
        {
            IEnumerable<PropertyInfo> propertyInfos = typeof(TEntity).GetProperties()
                .Where(item=>item.GetCustomAttribute<IgnoreInMappingAttribute>() is null);
            StringBuilder columnNamesBuilder = new();
            StringBuilder valuesPlaceholderBuilder = new();
            foreach(PropertyInfo propertyInfo in propertyInfos )
            {
                columnNamesBuilder.Append($"{propertyInfo.Name},");
                valuesPlaceholderBuilder.Append($"@{propertyInfo.Name},");
            }
            columnNamesBuilder.Remove(columnNamesBuilder.Length - 1,1);
            valuesPlaceholderBuilder.Remove(valuesPlaceholderBuilder.Length - 1, 1);
            string insertSQL = $"INSERT INTO {tableName} ({columnNamesBuilder}) VALUES ({valuesPlaceholderBuilder})";
            await _connection.ExecuteAsync(insertSQL, entity);

        }

        public ValueTask DisposeAsync()
        { return ((IAsyncDisposable)_connection).DisposeAsync(); }
    }
}
