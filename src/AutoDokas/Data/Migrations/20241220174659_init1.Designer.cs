﻿// <auto-generated />
using System;
using AutoDokas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AutoDokas.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241220174659_init1")]
    partial class init1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("AutoDokas.Data.Models.VehicleContract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("VehicleContracts");
                });

            modelBuilder.Entity("AutoDokas.Data.Models.VehicleContract", b =>
                {
                    b.OwnsOne("AutoDokas.Data.Models.VehicleContract+Vehicle", "VehicleInfo", b1 =>
                        {
                            b1.Property<Guid>("VehicleContractId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("AdditionalInformation")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Defects")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<bool>("HasBeenDamaged")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("IdentificationNumber")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<bool>("IsInspected")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Make")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<int>("Millage")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("PriorDamagesKnown")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("RegistrationNumber")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Sdk")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("VehicleContractId");

                            b1.ToTable("VehicleContracts");

                            b1.WithOwner()
                                .HasForeignKey("VehicleContractId");
                        });

                    b.OwnsOne("AutoDokas.Data.Models.VehicleContract+PartyInfo", "BuyerInfo", b1 =>
                        {
                            b1.Property<Guid>("VehicleContractId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Address")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Code")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Email")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("IsCompany")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Phone")
                                .HasColumnType("TEXT");

                            b1.HasKey("VehicleContractId");

                            b1.ToTable("VehicleContracts");

                            b1.WithOwner()
                                .HasForeignKey("VehicleContractId");
                        });

                    b.OwnsOne("AutoDokas.Data.Models.VehicleContract+PartyInfo", "SellerInfo", b1 =>
                        {
                            b1.Property<Guid>("VehicleContractId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Address")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Code")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Email")
                                .HasColumnType("TEXT");

                            b1.Property<bool>("IsCompany")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Phone")
                                .HasColumnType("TEXT");

                            b1.HasKey("VehicleContractId");

                            b1.ToTable("VehicleContracts");

                            b1.WithOwner()
                                .HasForeignKey("VehicleContractId");
                        });

                    b.Navigation("BuyerInfo");

                    b.Navigation("SellerInfo");

                    b.Navigation("VehicleInfo");
                });
#pragma warning restore 612, 618
        }
    }
}