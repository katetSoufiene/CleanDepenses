using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DeviseFeatures.Queries
{
    public class GetAllDevisesQuery : IRequest<IEnumerable<Devise>>
    {

        public class GetAllDevisesQueryHandler : IRequestHandler<GetAllDevisesQuery, IEnumerable<Devise>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllDevisesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Devise>> Handle(GetAllDevisesQuery query, CancellationToken cancellationToken)
            {
                var DeviseList = await _context.Devises.ToListAsync();
                if (DeviseList == null)
                {
                    return null;
                }
                return DeviseList.AsReadOnly();
            }
        }
    }
}
