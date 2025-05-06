using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerConsent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BuyerInfo_HasConsented",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SellerInfo_HasConsented",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerInfo_HasConsented",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "SellerInfo_HasConsented",
                table: "VehicleContracts");
        }
    }
}
