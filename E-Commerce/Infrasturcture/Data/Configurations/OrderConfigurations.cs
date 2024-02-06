using Core.Entities.OrderAggergate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturcture.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(order => order.ShippingAddress, address => address.WithOwner());

            builder.Property(order => order.OrderStatus)
                   .HasConversion(
                          status => status.ToString(),
                          value => (OrderStatus)Enum.Parse(typeof(OrderStatus), value)
                                 );

            builder.HasMany(order => order.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
