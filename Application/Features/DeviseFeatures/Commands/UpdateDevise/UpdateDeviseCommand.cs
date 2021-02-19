using Application.Interfaces;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DeviseFeatures.Commands
{
    public class UpdateDeviseCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public class UpdateDeviseCommandHandler : IRequestHandler<UpdateDeviseCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateDeviseCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateDeviseCommand command, CancellationToken cancellationToken)
            {
                var Devise = _context.Devises.Find(command.Id);

                if (Devise == null)
                {
                    return default;
                }
                else
                {
                    Devise.Code = command.Code;
                    Devise.Name = command.Name;
                    await _context.SaveChangesAsync();
                    return Devise.Id;
                }
            }
        }
    }
}
