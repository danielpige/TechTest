using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Users.Commands.UpdateUserCommand
{
    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(120);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(20)
                .Matches(@"^[0-9+\-\s]+$")
                .WithMessage("Phone contiene caracteres inválidos.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.CountryId).GreaterThan(0);
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.MunicipalityId).GreaterThan(0);
        }
    }
}
