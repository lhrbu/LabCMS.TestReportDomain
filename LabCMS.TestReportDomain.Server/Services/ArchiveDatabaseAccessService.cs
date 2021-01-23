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
    public class ArchiveDatabaseAccessService:IAsyncDisposable
    {
        private readonly NpgsqlConnection _connection;
        public ArchiveDatabaseAccessService(string connectionString= "Host=localhost;Username=postgres;Database=usage_records")
        { 
            _connection = new NpgsqlConnection(connectionString);
        }
        

        public async ValueTask<IEnumerable<object>> DynamicGetAllAsync(string tableName)=>
            await _connection.QueryAsync($"SELECT * FROM \"{tableName}\";");
        public async ValueTask<IEnumerable<TEntity>> GetAllAsync<TEntity>(string tableName) =>
            await _connection.QueryAsync<TEntity>($"SELECT {BuildColumnNames<TEntity>()} FROM \"{tableName}\"");
        public async ValueTask<IEnumerable<object>> GetAllAsync(string tableName, Type type) =>
            await _connection.QueryAsync($"SELECT {BuildColumnNames(type)} FROM \"{tableName}\"");
        public async ValueTask InsertAsync<TEntity>(string tableName, TEntity entity)
        {
            string insertStatement = BuildInsertStatement<TEntity>(tableName);
            await ExecuteWithTransactionAsync(insertStatement, entity);
        }
        public async ValueTask InsertAsync(string tableName,Type type, object entity)
        {
            string insertStatement = BuildInsertStatement(tableName, type);
            await ExecuteWithTransactionAsync(insertStatement, entity);
        }

        public async ValueTask InsertManyAsync<TEntity>(string tableName,params TEntity[] entities)
        {
            string insertStatement = BuildInsertStatement<TEntity>(tableName);
            await ExecuteWithTransactionAsync(insertStatement,entities);
        }
        public async ValueTask InsertManyAsync(string tableName, Type type,params object[] entities)
        {
            string insertStatement = BuildInsertStatement(tableName, type);
            await ExecuteWithTransactionAsync(insertStatement, entities);
        }
        
        /// <summary>
        /// Can't update columnName property in the Entity!
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="columnName"></param>
        /// <param name="predictation"></param>
        /// <returns></returns>
        public async ValueTask UpdateAsync<TEntity,TProperty>(string tableName,TEntity entity,string columnName, TProperty predictation)
        {
            string updateStatement = BuildUpdateStatement<TEntity>(tableName, columnName);
            await ExecuteWithTransactionAsync(updateStatement, entity);
        }

        public async ValueTask UpdateAsync(string tableName,Type type,object entity,string columnName,object predictation)
        {
            string updateStatement = BuildUpdateStatement(tableName, type, columnName);
            await ExecuteWithTransactionAsync(updateStatement, entity);
        }

        public async ValueTask DeleteAsync<TProperty>(string tableName,string columnName,TProperty predictation)
        {
            string deleteStatement = BuildDeleteStatement(tableName, columnName);
            await ExecuteWithTransactionAsync(deleteStatement, predictation!);
        }
        public async ValueTask DeleteAsync(string tableName,string columnName,object predictation)
        {
            string deleteStatement = BuildDeleteStatement(tableName, columnName);
            await ExecuteWithTransactionAsync(deleteStatement, predictation);
        }

        private async ValueTask ExecuteWithTransactionAsync(string statement,object param)
        {
            if(_connection.State is ConnectionState.Closed)
            { await _connection.OpenAsync(); }
            await using NpgsqlTransaction transaction = await _connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await _connection.ExecuteAsync(statement, param, transaction);
            await transaction.CommitAsync();
        }

        private string BuildColumnNames<TEntity>() => BuildColumnNames(typeof(TEntity));
        private string BuildColumnNames(Type type)
        {
            IEnumerable<PropertyInfo> propertyInfos = type.GetProperties()
                .Where(item => item.GetCustomAttribute<IgnoreInMappingAttribute>() is null);
            StringBuilder columnNamesBuilder = new();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                columnNamesBuilder.Append($"\"{propertyInfo.Name}\",");
            }
            return columnNamesBuilder.Remove(columnNamesBuilder.Length - 1, 1).ToString();
        }
        private string BuildInsertStatement<TEntity>(string tableName) => BuildInsertStatement(tableName, typeof(TEntity));
        private string BuildInsertStatement(string tableName,Type type)
        {
            IEnumerable<PropertyInfo> propertyInfos = type.GetProperties()
                .Where(item => item.GetCustomAttribute<IgnoreInMappingAttribute>() is null);
            StringBuilder columnNamesBuilder = new();
            StringBuilder valuesPlaceholderBuilder = new();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                columnNamesBuilder.Append($"\"{propertyInfo.Name}\",");
                valuesPlaceholderBuilder.Append($"@{propertyInfo.Name},");
            }
            columnNamesBuilder.Remove(columnNamesBuilder.Length - 1, 1);
            valuesPlaceholderBuilder.Remove(valuesPlaceholderBuilder.Length - 1, 1);
            return $"INSERT INTO \"{tableName}\" ({columnNamesBuilder}) VALUES ({valuesPlaceholderBuilder})";
        }
        private string BuildDeleteStatement(string tableName, string columnName) =>
            $"DELETE FROM \"{tableName}\" WHERE \"{columnName}\"=@{columnName}";
        private string BuildUpdateStatement<TEntity>(string tableName, string columnName) =>
            BuildUpdateStatement(tableName, typeof(TEntity), columnName);
        private string BuildUpdateStatement(string tableName,Type type,string columnName)
        {
            IEnumerable<PropertyInfo> propertyInfos = type.GetProperties()
               .Where(item => item.GetCustomAttribute<IgnoreInMappingAttribute>() is null);
            StringBuilder updateColumnsBuilder = new();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name != columnName)
                {
                    updateColumnsBuilder.Append($"\"{propertyInfo.Name}\"=@{propertyInfo.Name},");
                }
            }
            updateColumnsBuilder.Remove(updateColumnsBuilder.Length - 1, 1);
            return $"UPDATE \"{tableName}\" SET {updateColumnsBuilder} WHERE \"{columnName}\"=@{columnName}";
        }
        public ValueTask DisposeAsync()
        { return ((IAsyncDisposable)_connection).DisposeAsync(); }
    }
}
