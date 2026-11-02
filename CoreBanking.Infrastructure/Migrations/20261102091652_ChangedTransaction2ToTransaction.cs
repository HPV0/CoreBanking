using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTransaction2ToTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transactions2_accounts_account_from_id",
                table: "transactions2");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions2_accounts_account_to_id",
                table: "transactions2");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions_details",
                table: "transactions2");

            migrationBuilder.RenameTable(
                name: "transactions2",
                newName: "transactions");

            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_to_id",
                table: "transactions",
                newName: "ix_transactions_account_to_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_from_id",
                table: "transactions",
                newName: "ix_transactions_account_from_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_accounts_account_from_id",
                table: "transactions",
                column: "account_from_id",
                principalTable: "accounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_accounts_account_to_id",
                table: "transactions",
                column: "account_to_id",
                principalTable: "accounts",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transactions_accounts_account_from_id",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_accounts_account_to_id",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "transactions2");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_account_to_id",
                table: "transactions2",
                newName: "ix_transactions2_account_to_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_account_from_id",
                table: "transactions2",
                newName: "ix_transactions2_account_from_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions_details",
                table: "transactions2",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions2_accounts_account_from_id",
                table: "transactions2",
                column: "account_from_id",
                principalTable: "accounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions2_accounts_account_to_id",
                table: "transactions2",
                column: "account_to_id",
                principalTable: "accounts",
                principalColumn: "id");
        }
    }

}
