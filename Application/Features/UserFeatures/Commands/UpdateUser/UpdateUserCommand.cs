using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class UpdateUserCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int DeviseId { get; set; }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateUserCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                var User = _context.Users.Find(command.Id);

                if (User == null)
                {
                    return default;
                }
                else
                {
                    User.FirstName = command.FirstName;
                    User.LastName = command.LastName;
                    User.DeviseId = command.DeviseId;
                    User.Devise = _context.Devises.Find(command.DeviseId);
                    await _context.SaveChangesAsync();
                    return User.Id;
                }
            }
        }
    }
}
