using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Interfaces;
using TechTest.Application.Users.Dtos;

namespace TechTest.Application.Users.Queries.GetUserById
{
    public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailsDto?>
    {
        private readonly IUserReadRepository _readRepo;

        public GetUserByIdQueryHandler(IUserReadRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public Task<UserDetailsDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
            => _readRepo.GetByIdAsync(request.UserId, ct);
    }
}
