using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking_Core_System.Migrations
{
    /// <inheritdoc />
    public partial class AddBankCardToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_DestinationAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_SourceAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankCards_BankCardId",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdempotencyKey",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TransactionId1",
                table: "LedgerEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IdempotencyKey",
                table: "Transactions",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReferenceNumber",
                table: "Transactions",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_TransactionId1",
                table: "LedgerEntries",
                column: "TransactionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId1",
                table: "LedgerEntries",
                column: "TransactionId1",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_DestinationAccountId",
                table: "Transactions",
                column: "DestinationAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_SourceAccountId",
                table: "Transactions",
                column: "SourceAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankCards_BankCardId",
                table: "Transactions",
                column: "BankCardId",
                principalTable: "BankCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId1",
                table: "LedgerEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_DestinationAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_SourceAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankCards_BankCardId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_IdempotencyKey",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReferenceNumber",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_TransactionId1",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "TransactionId1",
                table: "LedgerEntries");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdempotencyKey",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_DestinationAccountId",
                table: "Transactions",
                column: "DestinationAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_SourceAccountId",
                table: "Transactions",
                column: "SourceAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankCards_BankCardId",
                table: "Transactions",
                column: "BankCardId",
                principalTable: "BankCards",
                principalColumn: "Id");
        }
    }
}
