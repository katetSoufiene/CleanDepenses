using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            var devise1 = new Devise() { Id = 1, Code = "USD", Name = "Dollar américain" };
            var devise2 = new Devise() { Id = 2, Code = "RUB", Name = "Rouble russe" };

            modelBuilder.Entity<Devise>().HasData(devise1, devise2);

            var devises = new List<Devise>() { devise2, devise1 };


            modelBuilder.Entity<User>().HasData(
                             new User() { Id = 1, FirstName = "Anthony", LastName = "Anthony", DeviseId = 1 },
                             new User() { Id = 2, FirstName = "Natasha", LastName = "Romanova", DeviseId = 2 }
                            );

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Devise> Devises { get; set; }

        public DbSet<Depense> Depenses { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
