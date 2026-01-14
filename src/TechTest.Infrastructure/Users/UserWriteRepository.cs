using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Interfaces;
using TechTest.Infrastructure.Persistence;

namespace TechTest.Infrastructure.Users
{
    public sealed class UserWriteRepository : IUserWriteRepository
    {
        private readonly DbConnectionFactory _factory;

        public UserWriteRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<long> CreateAsync( string fullName, string phone, string address, int countryId, int departmentId, int municipalityId, CancellationToken ct)
        {
            using var conn = _factory.Create();
            conn.Open();

            var p = new DynamicParameters();
            p.Add("p_full_name", fullName, DbType.String);
            p.Add("p_phone", phone, DbType.String);
            p.Add("p_address", address, DbType.String);
            p.Add("p_country_id", countryId, DbType.Int32);
            p.Add("p_department_id", departmentId, DbType.Int32);
            p.Add("p_municipality_id", municipalityId, DbType.Int32);

            p.Add("p_user_id", 0L, DbType.Int64, direction: ParameterDirection.InputOutput);

            const string sql =
                "CALL public.sp_user_create(@p_full_name, @p_phone, @p_address, @p_country_id, @p_department_id, @p_municipality_id, @p_user_id);";

            await conn.ExecuteAsync(new CommandDefinition(sql, p, cancellationToken: ct));
            return p.Get<long>("p_user_id");
        }

        public async Task<bool> UpdateAsync(long userId, string fullName, string phone, string address, int countryId, int departmentId, int municipalityId, CancellationToken ct)
        {
            using var conn = _factory.Create();
            conn.Open();

            var p = new DynamicParameters();
            p.Add("p_user_id", userId, DbType.Int64, direction: ParameterDirection.Input);
            p.Add("p_full_name", fullName, DbType.String, direction: ParameterDirection.Input);
            p.Add("p_phone", phone, DbType.String, direction: ParameterDirection.Input);
            p.Add("p_address", address, DbType.String, direction: ParameterDirection.Input);
            p.Add("p_country_id", countryId, DbType.Int32, direction: ParameterDirection.Input);
            p.Add("p_department_id", departmentId, DbType.Int32, direction: ParameterDirection.Input);
            p.Add("p_municipality_id", municipalityId, DbType.Int32, direction: ParameterDirection.Input);

            p.Add("p_updated", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            var cmd = new CommandDefinition(
                "sp_user_update",
                p,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            );

            await conn.ExecuteAsync(cmd);
            return p.Get<bool>("p_updated");
        }

        public async Task<bool> DeleteAsync(long userId, CancellationToken ct)
        {
            using var conn = _factory.Create();
            conn.Open();

            var p = new DynamicParameters();
            p.Add("p_user_id", userId, DbType.Int64, direction: ParameterDirection.Input);

            p.Add("p_deleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            var cmd = new CommandDefinition(
                "sp_user_delete",
                p,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            );

            await conn.ExecuteAsync(cmd);
            return p.Get<bool>("p_deleted");
        }
    }
}
