using Domain.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, address => address.WithOwner());

            builder.HasMany(O=>O.OrderItems)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(O=>O.DeliveryMethod)
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);

            builder.Property(O=>O.PaymentStatus)
                   .HasConversion(S=>S.ToString(),S=> Enum.Parse<OrderPaymentStatus>(S));

            builder.Property(O => O.SubTotal)
                   .HasColumnType("decimal(18,4)");
        }
    }
}
