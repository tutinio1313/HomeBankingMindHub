using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMindHub.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Clients_ClientGuid",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ClientGuid",
                table: "Accounts",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_ClientGuid",
                table: "Accounts",
                newName: "IX_Accounts_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Clients_ClientId",
                table: "Accounts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Clients_ClientId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Accounts",
                newName: "ClientGuid");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_ClientId",
                table: "Accounts",
                newName: "IX_Accounts_ClientGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Clients_ClientGuid",
                table: "Accounts",
                column: "ClientGuid",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
