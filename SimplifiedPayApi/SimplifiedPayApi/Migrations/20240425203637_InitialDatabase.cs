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
                name: "USERS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    IdentificationNumber = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    UserType = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.Id);
                    table.UniqueConstraint("UQ_USERS_EMAIL", x => x.Email);
                    table.UniqueConstraint("UQ_USERS_IDENTIFICATION_NUMBER", x => x.IdentificationNumber);
                });

            migrationBuilder.CreateTable(
                name: "DEPOSITS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "MONEY", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPOSITS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DEPOSITS_USERS",
                        column: x => x.UserId,
                        principalTable: "USERS",
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
                        name: "FK_TRANSACTIONS_USERS_PAYER",
                        column: x => x.PayerId,
                        principalTable: "USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TRANSACTIONS_USERS_RECEIVER",
                        column: x => x.ReceiverId,
                        principalTable: "USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_USERS_EMAIL",
                table: "USERS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_IDENTIFICATION_NUMBER",
                table: "USERS",
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
                name: "USERS");
        }
    }
}
