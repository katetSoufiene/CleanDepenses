using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DeviseFeatures.Commands
{

    public class CreateDeviseCommand : IRequest<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public class CreateDeviseCommandHandler : IRequestHandler<CreateDeviseCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper @object;

            public CreateDeviseCommandHandler(IApplicationDbContext context, AutoMapper.IMapper @object)
            {
                _context = context;
                this.@object = @object;
            }
            public async Task<int> Handle(CreateDeviseCommand command, CancellationToken cancellationToken)
            {
               
                
                var Devise = new Devise();
                Devise.Code = command.Code;
                Devise.Name = command.Name;

                _context.Devises.Add(Devise);

                await _context.SaveChangesAsync();

                return Devise.Id;
            }
        }
    }
}
