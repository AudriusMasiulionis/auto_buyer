using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class party_info_update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "BuyerInfo_IsCompany",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "VehicleInfo_AdditionalInformation",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleInfo_Defects",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VehicleInfo_HasBeenDamaged",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleInfo_IdentificationNumber",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleInfo_Make",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleInfo_Millage",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VehicleInfo_PriorDamagesKnown",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleInfo_RegistrationNumber",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleInfo_Sdk",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VehicleInfo_TechnicalInspectionValid",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleInfo_AdditionalInformation",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_Defects",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_HasBeenDamaged",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_IdentificationNumber",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_Make",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_Millage",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_PriorDamagesKnown",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_RegistrationNumber",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_Sdk",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "VehicleInfo_TechnicalInspectionValid",
                table: "VehicleContracts");

            migrationBuilder.AlterColumn<bool>(
                name: "BuyerInfo_IsCompany",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
