﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimplifiedPayApi.Context;

#nullable disable

namespace SimplifiedPayApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240508185228_AmountDepositModel")]
    partial class AmountDepositModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SimplifiedPayApi.Models.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("MONEY");

                    b.Property<int>("DepositorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepositorId");

                    b.ToTable("DEPOSITS", (string)null);
                });

            modelBuilder.Entity("SimplifiedPayApi.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("MONEY");

                    b.Property<int>("PayerId")
                        .HasColumnType("int");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("BIT");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.HasIndex("PayerId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("TRANSACTIONS", (string)null);
                });

            modelBuilder.Entity("SimplifiedPayApi.Models.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("MONEY");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email")
                        .HasName("UQ_WALLETS_EMAIL");

                    b.HasAlternateKey("IdentificationNumber")
                        .HasName("UQ_WALLETS_IDENTIFICATION_NUMBER");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("IX_WALLETS_EMAIL");

                    b.HasIndex("IdentificationNumber")
                        .IsUnique()
                        .HasDatabaseName("IX_WALLETS_IDENTIFICATION_NUMBER");

                    b.ToTable("WALLETS", (string)null);
                });

            modelBuilder.Entity("SimplifiedPayApi.Models.Deposit", b =>
                {
                    b.HasOne("SimplifiedPayApi.Models.Wallet", "Depositor")
                        .WithMany("Deposits")
                        .HasForeignKey("DepositorId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired()
                        .HasConstraintName("FK_DEPOSIT_WALLET_DEPOSITOR");

                    b.Navigation("Depositor");
                });

            modelBuilder.Entity("SimplifiedPayApi.Models.Transaction", b =>
                {
                    b.HasOne("SimplifiedPayApi.Models.Wallet", "Payer")
                        .WithMany("Transactions")
                        .HasForeignKey("PayerId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired()
                        .HasConstraintName("FK_TRANSACTIONS_WALLETS_PAYER");

                    b.HasOne("SimplifiedPayApi.Models.Wallet", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired()
                        .HasConstraintName("FK_TRANSACTIONS_WALLETS_RECEIVER");

                    b.Navigation("Payer");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("SimplifiedPayApi.Models.Wallet", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
