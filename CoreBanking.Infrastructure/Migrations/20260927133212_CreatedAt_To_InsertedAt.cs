using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatedAt_To_InsertedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TransactionsDetails",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Transactions",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CurrencyRates",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Currencies",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Clients",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Accounts",
                newName: "InsertedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "TransactionsDetails",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "Transactions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "CurrencyRates",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "Currencies",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "Clients",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "Accounts",
                newName: "CreatedAt");
        }
    }
}
