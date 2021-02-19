using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DepenseFeatures.Queries
{
    public class GetDepenseByIdQuery : IRequest<Depense>
    {
        public int Id { get; set; }
        public class GetDepenseByIdQueryHandler : IRequestHandler<GetDepenseByIdQuery, Depense>
        {
            private readonly IApplicationDbContext _context;
            public GetDepenseByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Depense> Handle(GetDepenseByIdQuery query, CancellationToken cancellationToken)
            {
                var Depense = _context.Depenses
                    .Where(a => a.Id == query.Id).FirstOrDefault();
                if (Depense == null) return null;
                return Depense;
            }
        }
    }
}
