using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeGIC.Bank.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AccountTxnCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTxn_BankAccounts_BankAccountId",
                table: "AccountTxn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTxn",
                table: "AccountTxn");

            migrationBuilder.RenameTable(
                name: "AccountTxn",
                newName: "AccountTxns");

            migrationBuilder.RenameIndex(
                name: "IX_AccountTxn_BankAccountId",
                table: "AccountTxns",
                newName: "IX_AccountTxns_BankAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTxns",
                table: "AccountTxns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTxns_BankAccounts_BankAccountId",
                table: "AccountTxns",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTxns_BankAccounts_BankAccountId",
                table: "AccountTxns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTxns",
                table: "AccountTxns");

            migrationBuilder.RenameTable(
                name: "AccountTxns",
                newName: "AccountTxn");

            migrationBuilder.RenameIndex(
                name: "IX_AccountTxns_BankAccountId",
                table: "AccountTxn",
                newName: "IX_AccountTxn_BankAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTxn",
                table: "AccountTxn",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTxn_BankAccounts_BankAccountId",
                table: "AccountTxn",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
