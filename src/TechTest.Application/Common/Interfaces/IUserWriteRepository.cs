using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Common.Interfaces
{
    public interface IUserWriteRepository
    {
        Task<long> CreateAsync(string fullName,
            string phone,
            string address,
            int countryId,
            int departmentId,
            int municipalityId, CancellationToken ct);

        Task<bool> UpdateAsync(
        long userId,
        string fullName,
        string phone,
        string address,
        int countryId,
        int departmentId,
        int municipalityId,
        CancellationToken ct);

        Task<bool> DeleteAsync(long userId, CancellationToken ct);
    }
}
