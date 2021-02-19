using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DeviseFeatures.Commands
{
    public class UpdateDeviseCommandValidator : AbstractValidator<UpdateDeviseCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDeviseCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleSet("UpdateDevise", () => {

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

        public async Task<bool> BeUniqueCode(UpdateDeviseCommand model, string code, CancellationToken cancellationToken)
        {
            return await _context.Devises
                .Where(l => l.Id != model.Id)
                .AllAsync(l => l.Code != code);
        }

        public async Task<bool> BeUniqueName(UpdateDeviseCommand model, string name, CancellationToken cancellationToken)
        {
            return await _context.Devises
                .Where(l => l.Id != model.Id)
                .AllAsync(l => l.Name != name);
        }
    }
}
