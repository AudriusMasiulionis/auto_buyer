using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleContracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SellerInfo_Code = table.Column<string>(type: "TEXT", nullable: true),
                    SellerInfo_IsCompany = table.Column<bool>(type: "INTEGER", nullable: true),
                    SellerInfo_Phone = table.Column<string>(type: "TEXT", nullable: true),
                    SellerInfo_Name = table.Column<string>(type: "TEXT", nullable: true),
                    SellerInfo_Address = table.Column<string>(type: "TEXT", nullable: true),
                    SellerInfo_Email = table.Column<string>(type: "TEXT", nullable: true),
                    SellerInfo_SignatureData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    BuyerInfo_Code = table.Column<string>(type: "TEXT", nullable: true),
                    BuyerInfo_IsCompany = table.Column<bool>(type: "INTEGER", nullable: true),
                    BuyerInfo_Phone = table.Column<string>(type: "TEXT", nullable: true),
                    BuyerInfo_Name = table.Column<string>(type: "TEXT", nullable: true),
                    BuyerInfo_Address = table.Column<string>(type: "TEXT", nullable: true),
                    BuyerInfo_Email = table.Column<string>(type: "TEXT", nullable: true),
                    BuyerInfo_SignatureData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    VehicleInfo_Sdk = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleInfo_Make = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleInfo_RegistrationNumber = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleInfo_Millage = table.Column<int>(type: "INTEGER", nullable: true),
                    VehicleInfo_IdentificationNumber = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleInfo_IsInspected = table.Column<bool>(type: "INTEGER", nullable: true),
                    VehicleInfo_HasBeenDamaged = table.Column<bool>(type: "INTEGER", nullable: true),
                    VehicleInfo_PriorDamagesKnown = table.Column<bool>(type: "INTEGER", nullable: true),
                    VehicleInfo_Defects = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleInfo_AdditionalInformation = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentInfo_Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    PaymentInfo_PaymentMethod = table.Column<int>(type: "INTEGER", nullable: true),
                    PaymentInfo_PaymentAtContractFormation = table.Column<bool>(type: "INTEGER", nullable: true),
                    PaymentInfo_PaymentDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    PaymentInfo_TransferInsurance = table.Column<bool>(type: "INTEGER", nullable: true),
                    PaymentInfo_AdditionalInformation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleContracts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleContracts");
        }
    }
}
