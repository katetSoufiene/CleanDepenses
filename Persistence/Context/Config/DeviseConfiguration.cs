using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Context.Config
{
    public class DeviseConfiguration : IEntityTypeConfiguration<Devise>
    {
        public void Configure(EntityTypeBuilder<Devise> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

        }
    }
}