using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechTest.Api.Contracts;
using TechTest.Application.Users.Commands.CreateUser;
using TechTest.Application.Users.Commands.DeleteUser;
using TechTest.Application.Users.Commands.UpdateUserCommand;
using TechTest.Application.Users.Queries.GetAllUsers;
using TechTest.Application.Users.Queries.GetUserById;
using TechTest.Application.Users.Dtos;

namespace TechTest.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender) => _sender = sender;

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreateUserResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken ct)
        {
            var result = await _sender.Send(command, ct);

            var response = ApiResponse<CreateUserResponse>.Created(
                result,
                "Usuario creado exitosamente."
            );

            return CreatedAtAction(nameof(GetById), new { id = result.UserId }, response);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<UserDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken ct)
        {
            var result = await _sender.Send(new GetUserByIdQuery(id), ct);
            if (result is null) return NotFound();

            return Ok(ApiResponse<UserDetailsDto>.Ok(result, "Consulta exitosa."));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UserDetailsDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int limit = 100,
            [FromQuery] int offset = 0,
            CancellationToken ct = default)
        {
            var result = await _sender.Send(new GetAllUsersQuery(limit, offset), ct);
            return Ok(ApiResponse<IReadOnlyList<UserDetailsDto>>.Ok(result, "Consulta exitosa."));
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateUserCommand command, CancellationToken ct)
        {
            var cmd = command with { UserId = id };
            await _sender.Send(cmd, ct);

            return Ok(ApiResponse.Ok("Usuario actualizado exitosamente."));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct)
        {
            await _sender.Send(new DeleteUserCommand(id), ct);
            return Ok(ApiResponse.Ok("Usuario eliminado exitosamente."));
        }
    }
}
