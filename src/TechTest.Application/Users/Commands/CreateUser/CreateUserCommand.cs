using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand(
        string FullName,
        string Phone,
        int CountryId,
        int DepartmentId,
        int MunicipalityId,
        string Address
    ) : IRequest<CreateUserResponse>;

    public sealed record CreateUserResponse(long UserId);
}
