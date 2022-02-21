﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.AggregateModels.BuyerAggregate;
using Order.Infrastructure.Context;

namespace Order.Infrastructure.EntityConfigurations
{
    public class BuyerEntityConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("buyers", OrderDbContext.DefaultSchema);

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Ignore(b => b.DomainEvents);

            builder.HasMany(b => b.PaymentMethods)
                .WithOne()
                .HasForeignKey(i => i.Id)
                .OnDelete(DeleteBehavior.Cascade);

            var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}