using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class CreateUserCommand : IRequest<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int DeviseId { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateUserCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
            {
                var User = new User();
                User.FirstName = command.FirstName;
                User.LastName = command.LastName;
                User.DeviseId = command.DeviseId;
                User.Devise = _context.Devises.Find(command.DeviseId);

                _context.Users.Add(User);

                await _context.SaveChangesAsync();

                return User.Id;
            }
        }
    }
}
