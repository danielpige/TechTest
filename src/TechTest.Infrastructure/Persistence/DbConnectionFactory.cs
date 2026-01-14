using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Infrastructure.Persistence
{
    public sealed class DbConnectionFactory
    {
        private readonly string _cs;

        public DbConnectionFactory(IConfiguration config)
        {
            _cs = config.GetConnectionString("Default")
                ?? throw new InvalidOperationException("ConnectionStrings:Default no está configurado.");
        }

        public IDbConnection Create() => new NpgsqlConnection(_cs);
    }
}
