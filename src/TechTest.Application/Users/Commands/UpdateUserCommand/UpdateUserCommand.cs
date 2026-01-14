using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Users.Commands.UpdateUserCommand
{
    public sealed record UpdateUserCommand(
        long UserId,
        string FullName,
        string Phone,
        int CountryId,
        int DepartmentId,
        int MunicipalityId,
        string Address
    ) : IRequest;

}
