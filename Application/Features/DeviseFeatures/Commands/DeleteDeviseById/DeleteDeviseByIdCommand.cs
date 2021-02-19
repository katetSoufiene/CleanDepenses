using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DeviseFeatures.Commands
{
    public class DeleteDeviseByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteDeviseByIdCommandHandler : IRequestHandler<DeleteDeviseByIdCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public DeleteDeviseByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteDeviseByIdCommand command, CancellationToken cancellationToken)
            {
                var devise = await _context.Devises.Where(a => a.Id == command.Id).FirstOrDefaultAsync();
                if (devise == null) return default;
                _context.Devises.Remove(devise);
                await _context.SaveChangesAsync();
                return devise.Id;
            }
        }
    }
}
