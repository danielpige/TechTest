using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Interfaces;

namespace TechTest.Application.Users.Commands.CreateUser
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserWriteRepository _writeRepo;

        public CreateUserCommandHandler(IUserWriteRepository writeRepo)
        {
            _writeRepo = writeRepo;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var userId = await _writeRepo.CreateAsync(
                request.FullName,
                request.Phone,
                request.Address,
                request.CountryId,
                request.DepartmentId,
                request.MunicipalityId,
                ct);

            return new CreateUserResponse(userId);
        }
    }
}
