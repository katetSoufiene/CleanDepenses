using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DeviseFeatures.Commands
{
    public class CreateDeviseCommandValidator : AbstractValidator<CreateDeviseCommand>
    {
        private readonly IApplicationDbContext _context;              

        public CreateDeviseCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleSet("CreateDevise", () => {
                RuleFor(v => v.Code)
              .NotEmpty().WithMessage("Code is required.")
              .Length(3).WithMessage("Code must be 3 characters.")
              .MustAsync(BeUniqueCode).WithMessage("The specified Code already exists.");

                RuleFor(v => v.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(200).WithMessage("Code must not exceed 200 characters.")
                    .MustAsync(BeUniqueName).WithMessage("The specified Name already exists.");

            });          
        }

        public async Task<bool> BeUniqueCode(string code, CancellationToken cancellationToken)
        {
            return await _context.Devises
                .AllAsync(l => l.Code != code);
        }

        public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return await _context.Devises
                .AllAsync(l => l.Name != name);
        }
    }
}
