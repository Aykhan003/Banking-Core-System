using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking_Core_System.Migrations
{
    /// <inheritdoc />
    public partial class m_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankCardId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankCardId",
                table: "Transactions",
                column: "BankCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankCards_BankCardId",
                table: "Transactions",
                column: "BankCardId",
                principalTable: "BankCards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankCards_BankCardId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BankCardId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BankCardId",
                table: "Transactions");
        }
    }
}
