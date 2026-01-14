using Dapper;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Interfaces;
using TechTest.Application.Users.Dtos;
using TechTest.Infrastructure.Persistence;

namespace TechTest.Infrastructure.Users
{
    public sealed class UserReadRepository : IUserReadRepository
    {
        private readonly DbConnectionFactory _factory;

        public UserReadRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<UserDetailsDto?> GetByIdAsync(long userId, CancellationToken ct)
        {
            using var conn = _factory.Create();
            conn.Open();

            var p = new DynamicParameters();
            p.Add("p_user_id", userId, DbType.Int64, direction: ParameterDirection.Input);

            p.Add("o_user_id", dbType: DbType.Int64, direction: ParameterDirection.Output);
            p.Add("o_full_name", dbType: DbType.String, size: 120, direction: ParameterDirection.Output);
            p.Add("o_phone", dbType: DbType.String, size: 20, direction: ParameterDirection.Output);
            p.Add("o_address", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            p.Add("o_country_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("o_country_name", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
            p.Add("o_department_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("o_department_name", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
            p.Add("o_municipality_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("o_municipality_name", dbType: DbType.String, size: 120, direction: ParameterDirection.Output);
            p.Add("o_created_at", dbType: DbType.DateTimeOffset, direction: ParameterDirection.Output);

            var cmd = new CommandDefinition(
                "sp_user_get_by_id",
                p,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            );

            await conn.ExecuteAsync(cmd);

            var id = p.Get<long?>("o_user_id");
            if (id is null) return null;

            return new UserDetailsDto(
                UserId: id.Value,
                FullName: p.Get<string?>("o_full_name") ?? "",
                Phone: p.Get<string?>("o_phone") ?? "",
                Address: p.Get<string?>("o_address") ?? "",
                CountryId: p.Get<int?>("o_country_id") ?? 0,
                CountryName: p.Get<string?>("o_country_name") ?? "",
                DepartmentId: p.Get<int?>("o_department_id") ?? 0,
                DepartmentName: p.Get<string?>("o_department_name") ?? "",
                MunicipalityId: p.Get<int?>("o_municipality_id") ?? 0,
                MunicipalityName: p.Get<string?>("o_municipality_name") ?? "",
                CreatedAt: p.Get<DateTime?>("o_created_at") ?? default
            );
        }

        public async Task<IReadOnlyList<UserDetailsDto>> GetAllAsync(int limit, int offset, CancellationToken ct)
        {
            using var conn = (NpgsqlConnection)_factory.Create();
            await conn.OpenAsync(ct);

            await using var tx = await conn.BeginTransactionAsync(ct);

            const string cursorName = "users_cur";

            await using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "CALL sp_user_get_all(@p_limit, @p_offset, @p_cursor);";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new NpgsqlParameter("p_limit", NpgsqlDbType.Integer) { Value = limit });
                cmd.Parameters.Add(new NpgsqlParameter("p_offset", NpgsqlDbType.Integer) { Value = offset });

                cmd.Parameters.Add(new NpgsqlParameter("p_cursor", NpgsqlDbType.Refcursor)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = cursorName
                });

                await cmd.ExecuteNonQueryAsync(ct);
            }

            var rows = await conn.QueryAsync<UserDetailsDto>(
                new CommandDefinition($"FETCH ALL IN {cursorName};", transaction: tx, cancellationToken: ct)
            );

            await conn.ExecuteAsync(
                new CommandDefinition($"CLOSE {cursorName};", transaction: tx, cancellationToken: ct)
            );

            await tx.CommitAsync(ct);

            return rows.AsList();
        }
    }
}
