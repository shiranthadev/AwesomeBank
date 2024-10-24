using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeGIC.Bank.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeAccountTxnIdAuto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AccountTxns",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "TxnSequenceId",
                table: "AccountTxns",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TxnSequenceId",
                table: "AccountTxns");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AccountTxns",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
