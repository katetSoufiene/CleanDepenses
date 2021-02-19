using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Context.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.FirstName)
                .IsRequired();

            builder.Property(p => p.LastName)
                .IsRequired();

            builder
                .HasMany<Depense>(u => u.Depenses)
                .WithOne(d => d.User)
                .HasForeignKey(s => s.UserId)
               ;

            builder
               .HasOne<Devise>()
               .WithMany()
               .HasForeignKey(s => s.DeviseId)
               ;
        }
    }
}