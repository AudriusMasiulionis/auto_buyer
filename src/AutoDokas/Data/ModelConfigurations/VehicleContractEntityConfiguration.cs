using AutoDokas.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoDokas.Data.ModelConfigurations;

public class VehicleContractEntityConfiguration : IEntityTypeConfiguration<VehicleContract>
{
    public void Configure(EntityTypeBuilder<VehicleContract> builder)
    {
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.BuyerInfo);
        builder.OwnsOne(x => x.SellerInfo);
        builder.OwnsOne(x => x.VehicleInfo, entity =>
        {
            entity.Property(p => p.Defects).HasConversion<string>();
        });
        builder.OwnsOne(x => x.PaymentInfo);
        builder.OwnsOne(x => x.Origin);
    }
}