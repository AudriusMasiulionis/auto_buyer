using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleInfo_Defects",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleInfo_Defects",
                table: "VehicleContracts");
        }
    }
}
