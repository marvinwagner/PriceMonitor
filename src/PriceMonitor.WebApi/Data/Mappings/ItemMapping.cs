using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceMonitor.WebApi.Models;

namespace PriceMonitor.WebApi.Data.Mappings
{
    public class ItemMapping : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Url)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(e => e.CurrentInCashValue).HasColumnType("money");
            builder.Property(e => e.CurrentNormalValue).HasColumnType("money");
            builder.Property(e => e.CurrentFullValue).HasColumnType("money");

            builder.ToTable("Item");
        }
    }
}
