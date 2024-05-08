using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplifiedPayApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WALLETS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    IdentificationNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Balance = table.Column<decimal>(type: "MONEY", nullable: false),
                    UserType = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WALLETS", x => x.Id);
                    table.UniqueConstraint("UQ_WALLETS_EMAIL", x => x.Email);
                    table.UniqueConstraint("UQ_WALLETS_IDENTIFICATION_NUMBER", x => x.IdentificationNumber);
                });

            migrationBuilder.CreateTable(
                name: "DEPOSITS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPOSITS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DEPOSIT_WALLET_DEPOSITOR",
                        column: x => x.DepositorId,
                        principalTable: "WALLETS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TRANSACTIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "MONEY", nullable: false),
                    PayerId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "BIT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACTIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TRANSACTIONS_WALLETS_PAYER",
                        column: x => x.PayerId,
                        principalTable: "WALLETS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TRANSACTIONS_WALLETS_RECEIVER",
                        column: x => x.ReceiverId,
                        principalTable: "WALLETS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEPOSITS_DepositorId",
                table: "DEPOSITS",
                column: "DepositorId");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTIONS_PayerId",
                table: "TRANSACTIONS",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTIONS_ReceiverId",
                table: "TRANSACTIONS",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_WALLETS_EMAIL",
                table: "WALLETS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WALLETS_IDENTIFICATION_NUMBER",
                table: "WALLETS",
                column: "IdentificationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEPOSITS");

            migrationBuilder.DropTable(
                name: "TRANSACTIONS");

            migrationBuilder.DropTable(
                name: "WALLETS");
        }
    }
}
