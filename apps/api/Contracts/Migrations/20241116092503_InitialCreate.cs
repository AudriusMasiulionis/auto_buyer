using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Contracts.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Buyer_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Buyer_Address = table.Column<string>(type: "TEXT", nullable: true),
                    Buyer_Company = table.Column<bool>(type: "INTEGER", nullable: true),
                    Buyer_Code = table.Column<string>(type: "TEXT", nullable: true),
                    Buyer_Email = table.Column<string>(type: "TEXT", nullable: true),
                    Buyer_PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Seller_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Seller_Address = table.Column<string>(type: "TEXT", nullable: true),
                    Seller_Company = table.Column<bool>(type: "INTEGER", nullable: true),
                    Seller_Code = table.Column<string>(type: "TEXT", nullable: true),
                    Seller_Email = table.Column<string>(type: "TEXT", nullable: true),
                    Seller_PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_SDK = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_Make = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_Model = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_StateRegistrationNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_Mileage = table.Column<int>(type: "INTEGER", nullable: true),
                    Vehicle_VIN = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_RegistrationCertificateNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_TechnicalInspectionValid = table.Column<bool>(type: "INTEGER", nullable: true),
                    Vehicle_HadOwnershipIncidents = table.Column<bool>(type: "INTEGER", nullable: true),
                    Vehicle_HadIncidentsBeforeOwnership = table.Column<bool>(type: "INTEGER", nullable: true),
                    Vehicle_Defects = table.Column<string>(type: "TEXT", nullable: true),
                    Vehicle_IncidentInformation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");
        }
    }
}
