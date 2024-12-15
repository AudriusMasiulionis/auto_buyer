using AutoDokas.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoDokas.Data.ModelConfigurations;

public class VehicleContractConfiguration : IEntityTypeConfiguration<VehicleContract>
{
    public void Configure(EntityTypeBuilder<VehicleContract> builder)
    {
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.BuyerInfo);
        builder.OwnsOne(x => x.SellerInfo);
        builder.OwnsOne(x => x.VehicleInfo, vehicle =>
        {
            vehicle.Property(p => p.Defects).HasConversion<string>();
        });
    }
}