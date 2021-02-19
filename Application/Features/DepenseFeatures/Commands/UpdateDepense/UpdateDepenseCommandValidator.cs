using Application.Interfaces;
using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DepenseFeatures.Commands
{
    public class UpdateDepenseCommandValidator : AbstractValidator<UpdateDepenseCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateDepenseCommandValidator(IApplicationDbContext context)
        {
            RuleSet("UpdateDepense", () =>
            {
                RuleFor(x => x.Id).Must(DepenseExist).WithMessage("Depense not found"); ;
                RuleFor(x => x.Commentaire).NotEmpty().WithMessage("Le commentaire est obligatoire.");
                RuleFor(x => x.Commentaire).NotNull().WithMessage("Le commentaire est obligatoire.");
                RuleFor(x => x.Date).NotNull().MustAsync(NotBeIntheFuture).WithMessage("Une dépense ne peut pas avoir une date dans le futur.");
                RuleFor(x => x.Date).MustAsync(LessThenThreeMonts).WithMessage("Une dépense ne peut pas être datée de plus de 3 mois.");
                RuleFor(x => x.DeviseId).MustAsync(BeSameDeviseAsUser).WithMessage("La devise de la dépense doit être identique à celle de l'utilisateur.");

                When(x => DateExist(x), () =>
                {
                    RuleFor(x => x.Montant)
                    .Must(BeDifferentAmount)
                    .WithMessage("Un utilisateur ne peut pas déclarer deux fois la même dépense (même date et même montant).");
                });

            });

            this.context = context;
        }

        private bool DepenseExist(int id)
        {
            return context.Depenses.Any(d => d.Id == id);
        }       

        private bool BeDifferentAmount(UpdateDepenseCommand command, double montant)
        {
            return !context.Depenses.Any(d => d.UserId == command.UserId && d.Montant == montant && d.Date == command.Date);
        }

        private bool DateExist(UpdateDepenseCommand command)
        {
            return context.Depenses.Any(d => d.UserId == command.UserId && d.Date == command.Date);
        }

        private Task<bool> BeSameDeviseAsUser(UpdateDepenseCommand command, int devise, CancellationToken arg2)
        {
            return Task.FromResult(context.Users.Find(command.UserId).DeviseId == devise);
        }

        private Task<bool> NotBeIntheFuture(DateTime date, CancellationToken arg2)
        {
            return Task.FromResult(date < DateTime.Now);
        }

        private Task<bool> LessThenThreeMonts(DateTime date, CancellationToken arg2)
        {
            return Task.FromResult(date > DateTime.Now.AddMonths(-3));
        }
    }
}
