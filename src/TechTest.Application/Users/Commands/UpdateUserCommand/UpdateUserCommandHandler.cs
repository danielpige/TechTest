using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Interfaces;
using TechTest.Application.Common.Exceptions;

namespace TechTest.Application.Users.Commands.UpdateUserCommand
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserWriteRepository _writeRepo;

        public UpdateUserCommandHandler(IUserWriteRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken ct)
        {
            var updated = await _writeRepo.UpdateAsync(
                request.UserId,
                request.FullName,
                request.Phone,
                request.Address,
                request.CountryId,
                request.DepartmentId,
                request.MunicipalityId,
                ct);

            if (!updated)
                throw new NotFoundException($"User con id {request.UserId} no existe.");
        }
    }
}
