using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceMonitor.WebApi.Models;

namespace PriceMonitor.WebApi.Data.Mappings
{
    public class ItemHistoryMapping : IEntityTypeConfiguration<ItemHistory>
    {
        public void Configure(EntityTypeBuilder<ItemHistory> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Item).WithMany(e => e.History).HasForeignKey(e => e.ItemId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.InCashValue).HasColumnType("money");
            builder.Property(e => e.NormalValue).HasColumnType("money");
            builder.Property(e => e.FullValue).HasColumnType("money");

            builder.ToTable("History");
        }
    }
}
