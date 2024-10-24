using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeGIC.Bank.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BankAccountCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountTxn",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TxnDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    BankAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    TxnType = table.Column<char>(type: "TEXT", nullable: false),
                    TxnAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTxn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountTxn_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTxn_BankAccountId",
                table: "AccountTxn",
                column: "BankAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTxn");

            migrationBuilder.DropTable(
                name: "BankAccounts");
        }
    }
}
