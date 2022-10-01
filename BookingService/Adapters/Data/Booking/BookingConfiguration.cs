using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Domain.Entities;

namespace Data.Booking
{
    public class BookingConfiguration : IEntityTypeConfiguration<Entities.Booking>
    {
        public void Configure(EntityTypeBuilder<Entities.Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Room).Property(x => x.Id);
            builder.OwnsOne(x => x.Guest).Property(x => x.Id);
        }
    }
}
