using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class party_info_update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleInfo_TechnicalInspectionValid",
                table: "VehicleContracts",
                newName: "VehicleInfo_IsInspected");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleInfo_IsInspected",
                table: "VehicleContracts",
                newName: "VehicleInfo_TechnicalInspectionValid");
        }
    }
}
