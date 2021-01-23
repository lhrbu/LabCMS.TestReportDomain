using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Server.Services
{
    public class DatabaseAccessService:IAsyncDisposable
    {
        public NpgsqlConnection DbConnection { get; }
        public DatabaseAccessService(string connectionString = "Host=localhost;Username=postgres;Database=usage_records")
        {
            DbConnection = new NpgsqlConnection(connectionString);
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)DbConnection).DisposeAsync();
        }
    }
}
