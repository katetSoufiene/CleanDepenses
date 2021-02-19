using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Devise> Devises { get; set; }
        DbSet<Depense> Depenses { get; set; }

        Task<int> SaveChangesAsync();
    }
}
