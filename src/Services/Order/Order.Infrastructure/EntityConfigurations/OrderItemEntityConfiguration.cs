using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.AggregateModels.OrderAggregate;
using Order.Infrastructure.Context;

namespace Order.Infrastructure.EntityConfigurations
{
    public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("orderItems", OrderDbContext.DefaultSchema);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder.Property(o => o.ProductName).HasMaxLength(300).IsRequired();
            builder.Property(o => o.PictureUrl).HasMaxLength(2000);
            builder.Property(o => o.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Ignore(o => o.DomainEvents);
            builder.Property<Guid>("OrderId").IsRequired();
        }
    }
}