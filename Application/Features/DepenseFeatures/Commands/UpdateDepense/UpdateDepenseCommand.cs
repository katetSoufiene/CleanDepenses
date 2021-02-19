using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DepenseFeatures.Commands
{

    public class UpdateDepenseCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string NatureDepense { get; set; }
        public double Montant { get; set; }
        public int DeviseId { get; set; }
        public string Commentaire { get; set; }

        public class UpdateDepenseCommandHandler : IRequestHandler<UpdateDepenseCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper mapper;

            public UpdateDepenseCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                this.mapper = mapper;
            }

            public async Task<int> Handle(UpdateDepenseCommand command, CancellationToken cancellationToken)
            {
                var Depense = _context.Depenses.Find(command.Id);

                if (Depense == null)
                {
                    return default;
                }
                else
                {
                    Depense.UserId = command.UserId;
                    Depense.Date = command.Date;
                    Depense.NatureDepense = mapper.Map<NatureDepense>(command.NatureDepense);
                    Depense.Montant = command.Montant;
                    Depense.DeviseId = command.DeviseId;
                    Depense.Commentaire = command.Commentaire;

                    Depense.Devise = _context.Devises.Find(command.DeviseId);
                    Depense.User = _context.Users.Find(command.UserId);

                    await _context.SaveChangesAsync();
                    return Depense.Id;
                }
            }
        }
    }
}
