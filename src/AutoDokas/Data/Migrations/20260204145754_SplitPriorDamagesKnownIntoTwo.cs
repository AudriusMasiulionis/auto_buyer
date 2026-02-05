using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitPriorDamagesKnownIntoTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleInfo_PriorDamagesKnown",
                table: "VehicleContracts",
                newName: "VehicleInfo_DamagedDuringOwnership");

            migrationBuilder.AddColumn<bool>(
                name: "VehicleInfo_DamageIncidentsKnown",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleInfo_DamageIncidentsKnown",
                table: "VehicleContracts");

            migrationBuilder.RenameColumn(
                name: "VehicleInfo_DamagedDuringOwnership",
                table: "VehicleContracts",
                newName: "VehicleInfo_PriorDamagesKnown");
        }
    }
}
