using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class party_info : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerInfo_Address",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BuyerInfo_Code",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "BuyerInfo_IsCompany",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BuyerInfo_Name",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BuyerInfo_Phone",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerInfo_Address",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerInfo_Code",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SellerInfo_IsCompany",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SellerInfo_Name",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerInfo_Phone",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerInfo_Address",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "BuyerInfo_Code",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "BuyerInfo_IsCompany",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "BuyerInfo_Name",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "BuyerInfo_Phone",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "SellerInfo_Address",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "SellerInfo_Code",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "SellerInfo_IsCompany",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "SellerInfo_Name",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "SellerInfo_Phone",
                table: "VehicleContracts");
        }
    }
}
