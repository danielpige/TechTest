using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Users.Dtos
{
    public sealed record UserDetailsDto(
        long UserId,
        string FullName,
        string Phone,
        string Address,
        int CountryId,
        string CountryName,
        int DepartmentId,
        string DepartmentName,
        int MunicipalityId,
        string MunicipalityName,
        DateTime CreatedAt
    );
}
