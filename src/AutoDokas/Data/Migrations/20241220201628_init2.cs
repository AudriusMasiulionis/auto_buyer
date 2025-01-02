using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentInfo_AdditionalInformation",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PaymentInfo_PaymentAtContractFormation",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PaymentInfo_PaymentDate",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentInfo_PaymentMethod",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentInfo_Price",
                table: "VehicleContracts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PaymentInfo_TransferInsurance",
                table: "VehicleContracts",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentInfo_AdditionalInformation",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "PaymentInfo_PaymentAtContractFormation",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "PaymentInfo_PaymentDate",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "PaymentInfo_PaymentMethod",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "PaymentInfo_Price",
                table: "VehicleContracts");

            migrationBuilder.DropColumn(
                name: "PaymentInfo_TransferInsurance",
                table: "VehicleContracts");
        }
    }
}
