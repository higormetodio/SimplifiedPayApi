using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplifiedPayApi.Migrations
{
    /// <inheritdoc />
    public partial class AmountDepositModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "DEPOSITS",
                type: "MONEY",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "DEPOSITS");
        }
    }
}
