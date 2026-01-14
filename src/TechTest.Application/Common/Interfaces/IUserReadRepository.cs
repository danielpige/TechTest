using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Users.Dtos;

namespace TechTest.Application.Common.Interfaces
{
    public interface IUserReadRepository
    {
        Task<UserDetailsDto?> GetByIdAsync(long userId, CancellationToken ct);

        Task<IReadOnlyList<UserDetailsDto>> GetAllAsync(int limit, int offset, CancellationToken ct);
    }
}
