using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DeviseFeatures.Queries
{
    public class GetDeviseByIdQuery : IRequest<Devise>
    {
        public int Id { get; set; }
        public class GetDeviseByIdQueryHandler : IRequestHandler<GetDeviseByIdQuery, Devise>
        {
            private readonly IApplicationDbContext _context;
            public GetDeviseByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Devise> Handle(GetDeviseByIdQuery query, CancellationToken cancellationToken)
            {
                var Devise = _context.Devises.Where(a => a.Id == query.Id).FirstOrDefault();
                if (Devise == null) return null;
                return Devise;
            }
        }
    }
}
