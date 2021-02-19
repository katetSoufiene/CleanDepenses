using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DepenseFeatures.Commands
{


    public class CreateDepenseCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string NatureDepense { get; set; }
        public double Montant { get; set; }
        public int DeviseId { get; set; }
        public string Commentaire { get; set; }

        public class CreateDepenseCommandHandler : IRequestHandler<CreateDepenseCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper mapper;

            public CreateDepenseCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                this.mapper = mapper;
            }
            public async Task<int> Handle(CreateDepenseCommand command, CancellationToken cancellationToken)
            {               

                var Depense = new Depense();
                Depense.UserId = command.UserId;
                Depense.Date = command.Date;
                Depense.NatureDepense = mapper.Map<NatureDepense>(command.NatureDepense);
                Depense.Montant = command.Montant;
                Depense.DeviseId = command.DeviseId;
                Depense.Commentaire = command.Commentaire;

                Depense.Devise = _context.Devises.Find(command.DeviseId);
                Depense.User = _context.Users.Find(command.UserId);

                _context.Depenses.Add(Depense);

                await _context.SaveChangesAsync();

                return Depense.Id;
            }
        }
    }
}
