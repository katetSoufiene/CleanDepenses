using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Persistence.Context.Config
{
    public class DepenseConfiguration : IEntityTypeConfiguration<Depense>
    {
        public void Configure(EntityTypeBuilder<Depense> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Montant)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(p => p.NatureDepense)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<NatureDepense>());

            builder.Property(x => x.Date)
               .IsRequired()
               .HasColumnType("date");

            builder.Property(x => x.Commentaire)
              .IsRequired();

            builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}