using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int Id { get; set; }
        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
        {
            private readonly IApplicationDbContext _context;
            public GetUserByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<User> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
            {
                var User = _context.Users
                    .Where(a => a.Id == query.Id).FirstOrDefault();
                if (User == null) return null;
                return User;
            }
        }
    }
}
