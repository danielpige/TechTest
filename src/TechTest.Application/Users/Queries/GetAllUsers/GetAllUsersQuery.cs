using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Users.Dtos;

namespace TechTest.Application.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery(int Limit = 100, int Offset = 0) : IRequest<IReadOnlyList<UserDetailsDto>>;
}
