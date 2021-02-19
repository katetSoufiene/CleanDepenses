using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DepenseFeatures.Commands
{
    public class DeleteDepenseByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteDepenseByIdCommandHandler : IRequestHandler<DeleteDepenseByIdCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public DeleteDepenseByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteDepenseByIdCommand command, CancellationToken cancellationToken)
            {
                var Depense = await _context.Depenses.Where(a => a.Id == command.Id).FirstOrDefaultAsync();
                if (Depense == null) return default;
                _context.Depenses.Remove(Depense);
                await _context.SaveChangesAsync();
                return Depense.Id;
            }
        }
    }
}
