using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class DeleteUserByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public DeleteUserByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteUserByIdCommand command, CancellationToken cancellationToken)
            {
                var User = await _context.Users.Where(a => a.Id == command.Id).FirstOrDefaultAsync();
                if (User == null) return default;
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
                return User.Id;
            }
        }
    }
}
