using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNewIdToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_account_balances_accounts_new_id",
                table: "account_balances");

            migrationBuilder.DropForeignKey(
                name: "fk_accounts_clients_client_new_id",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_accounts_currencies_currency_new_id",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_currency_rates_currencies_currency_new_id",
                table: "currency_rates");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions2_accounts_account_from_new_id",
                table: "transactions2");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions2_accounts_account_to_new_id",
                table: "transactions2");

            migrationBuilder.RenameColumn(
                name: "account_to_new_id",
                table: "transactions2",
                newName: "account_to_id");

            migrationBuilder.RenameColumn(
                name: "account_from_new_id",
                table: "transactions2",
                newName: "account_from_id");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "transactions2",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_to_new_id",
                table: "transactions2",
                newName: "ix_transactions2_account_to_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_from_new_id",
                table: "transactions2",
                newName: "ix_transactions2_account_from_id");

            migrationBuilder.RenameColumn(
                name: "currency_new_id",
                table: "currency_rates",
                newName: "currency_id");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "currency_rates",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "ix_currency_rates_currency_new_id",
                table: "currency_rates",
                newName: "ix_currency_rates_currency_id");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "currencies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "clients",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "currency_new_id",
                table: "accounts",
                newName: "currency_id");

            migrationBuilder.RenameColumn(
                name: "client_new_id",
                table: "accounts",
                newName: "client_id");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_currency_new_id",
                table: "accounts",
                newName: "ix_accounts_currency_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_client_new_id",
                table: "accounts",
                newName: "ix_accounts_client_id");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "account_balances",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_account_balances_accounts_id",
                table: "account_balances",
                column: "id",
                principalTable: "accounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_accounts_clients_client_id",
                table: "accounts",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_accounts_currencies_currency_id",
                table: "accounts",
                column: "currency_id",
                principalTable: "currencies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_currency_rates_currencies_currency_id",
                table: "currency_rates",
                column: "currency_id",
                principalTable: "currencies",
                principalColumn: "id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_account_balances_accounts_id",
                table: "account_balances");

            migrationBuilder.DropForeignKey(
                name: "fk_accounts_clients_client_id",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_accounts_currencies_currency_id",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_currency_rates_currencies_currency_id",
                table: "currency_rates");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions2_accounts_account_from_id",
                table: "transactions2");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions2_accounts_account_to_id",
                table: "transactions2");

            migrationBuilder.RenameColumn(
                name: "account_to_id",
                table: "transactions2",
                newName: "account_to_new_id");

            migrationBuilder.RenameColumn(
                name: "account_from_id",
                table: "transactions2",
                newName: "account_from_new_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "transactions2",
                newName: "new_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_to_id",
                table: "transactions2",
                newName: "ix_transactions2_account_to_new_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_from_id",
                table: "transactions2",
                newName: "ix_transactions2_account_from_new_id");

            migrationBuilder.RenameColumn(
                name: "currency_id",
                table: "currency_rates",
                newName: "currency_new_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "currency_rates",
                newName: "new_id");

            migrationBuilder.RenameIndex(
                name: "ix_currency_rates_currency_id",
                table: "currency_rates",
                newName: "ix_currency_rates_currency_new_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "currencies",
                newName: "new_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "clients",
                newName: "new_id");

            migrationBuilder.RenameColumn(
                name: "currency_id",
                table: "accounts",
                newName: "currency_new_id");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "accounts",
                newName: "client_new_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "accounts",
                newName: "new_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_currency_id",
                table: "accounts",
                newName: "ix_accounts_currency_new_id");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_client_id",
                table: "accounts",
                newName: "ix_accounts_client_new_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "account_balances",
                newName: "new_id");

            migrationBuilder.AddForeignKey(
                name: "fk_account_balances_accounts_new_id",
                table: "account_balances",
                column: "new_id",
                principalTable: "accounts",
                principalColumn: "new_id");

            migrationBuilder.AddForeignKey(
                name: "fk_accounts_clients_client_new_id",
                table: "accounts",
                column: "client_new_id",
                principalTable: "clients",
                principalColumn: "new_id");

            migrationBuilder.AddForeignKey(
                name: "fk_accounts_currencies_currency_new_id",
                table: "accounts",
                column: "currency_new_id",
                principalTable: "currencies",
                principalColumn: "new_id");

            migrationBuilder.AddForeignKey(
                name: "fk_currency_rates_currencies_currency_new_id",
                table: "currency_rates",
                column: "currency_new_id",
                principalTable: "currencies",
                principalColumn: "new_id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions2_accounts_account_from_new_id",
                table: "transactions2",
                column: "account_from_new_id",
                principalTable: "accounts",
                principalColumn: "new_id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions2_accounts_account_to_new_id",
                table: "transactions2",
                column: "account_to_new_id",
                principalTable: "accounts",
                principalColumn: "new_id");
        }
    }
}
