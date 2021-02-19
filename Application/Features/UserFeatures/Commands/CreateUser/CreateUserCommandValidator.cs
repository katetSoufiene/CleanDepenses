using Application.Interfaces;
using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IApplicationDbContext context;

        public CreateUserCommandValidator(IApplicationDbContext context )
        {
            this.context = context;

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
            RuleFor(x => x.FirstName).NotNull().WithMessage("FirstName is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
            RuleFor(x => x.LastName).NotNull().WithMessage("LastName is required.");
            RuleFor(x => x.DeviseId).MustAsync(DeviseExist).WithMessage("DeviseId is required.");
        }

        private Task<bool> DeviseExist(int devise, CancellationToken arg2)
        {
            return Task.FromResult(context.Devises.Any(d => d.Id == devise));
        }
    }
}
