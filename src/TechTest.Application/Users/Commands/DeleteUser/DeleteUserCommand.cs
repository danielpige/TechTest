using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Users.Commands.DeleteUser
{
    public sealed record DeleteUserCommand(long UserId) : IRequest;
}
