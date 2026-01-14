using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Exceptions;
using TechTest.Application.Common.Interfaces;

namespace TechTest.Application.Users.Commands.DeleteUser
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserWriteRepository _writeRepo;

        public DeleteUserCommandHandler(IUserWriteRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken ct)
        {
            var ok = await _writeRepo.DeleteAsync(request.UserId, ct);

            if (!ok)
                throw new NotFoundException($"User con id {request.UserId} no existe.");
        }
    }
}
