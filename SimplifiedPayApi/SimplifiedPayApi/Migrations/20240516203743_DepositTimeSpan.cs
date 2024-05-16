using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplifiedPayApi.Migrations
{
    /// <inheritdoc />
    public partial class DepositTimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeSpan",
                table: "DEPOSITS",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpan",
                table: "DEPOSITS");
        }
    }
}
