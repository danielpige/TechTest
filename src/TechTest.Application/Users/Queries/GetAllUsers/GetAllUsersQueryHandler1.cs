using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Interfaces;
using TechTest.Application.Users.Dtos;

namespace TechTest.Application.Users.Queries.GetAllUsers
{
    public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserDetailsDto>>
    {
        private readonly IUserReadRepository _readRepo;

        public GetAllUsersQueryHandler(IUserReadRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public Task<IReadOnlyList<UserDetailsDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
            => _readRepo.GetAllAsync(request.Limit, request.Offset, ct);
    }
}
