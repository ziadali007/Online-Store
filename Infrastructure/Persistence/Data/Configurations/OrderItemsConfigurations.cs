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
    public class OrderItemsConfigurations : IEntityTypeConfiguration<OrderItem>
    {
       
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(item=>item.product,product=>product.WithOwner());

            builder.Property(item => item.Price)
                   .HasColumnType("decimal(18,4)");
        }
    }
}
