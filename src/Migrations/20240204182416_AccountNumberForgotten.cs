using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMindHub.Migrations
{
    /// <inheritdoc />
    public partial class AccountNumberForgotten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Accounts");
        }
    }
}
