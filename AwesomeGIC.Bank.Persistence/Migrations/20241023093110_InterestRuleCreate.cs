using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeGIC.Bank.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InterestRuleCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterestRules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    RuleDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    InterestRate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestRules", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestRules");
        }
    }
}
