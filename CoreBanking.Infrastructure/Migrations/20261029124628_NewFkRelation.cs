using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewFkRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "fk_transactions_details_accounts_account_from_id",
                table: "transactions2");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_details_accounts_account_to_id",
                table: "transactions2");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions_details",
                table: "transactions2");

            migrationBuilder.DropIndex(
                name: "ix_transactions2_account_from_id",
                table: "transactions2");

            migrationBuilder.DropIndex(
                name: "ix_transactions2_account_to_id",
                table: "transactions2");

            migrationBuilder.DropPrimaryKey(
                name: "pk_currency_rates",
                table: "currency_rates");

            migrationBuilder.DropIndex(
                name: "ix_currency_rates_currency_id",
                table: "currency_rates");

            migrationBuilder.DropPrimaryKey(
                name: "pk_currencies",
                table: "currencies");

            migrationBuilder.DropPrimaryKey(
                name: "pk_clients",
                table: "clients");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounts",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "ix_accounts_client_id",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "ix_accounts_currency_id",
                table: "accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_account_balances",
                table: "account_balances");

            migrationBuilder.AlterColumn<int>(
                name: "currency_new_id",
                table: "currency_rates",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions_details",
                table: "transactions2",
                column: "new_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_currency_rates",
                table: "currency_rates",
                column: "new_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_currencies",
                table: "currencies",
                column: "new_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_clients",
                table: "clients",
                column: "new_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounts",
                table: "accounts",
                column: "new_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_account_balances",
                table: "account_balances",
                column: "new_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions2_account_from_new_id",
                table: "transactions2",
                column: "account_from_new_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions2_account_to_new_id",
                table: "transactions2",
                column: "account_to_new_id");

            migrationBuilder.CreateIndex(
                name: "ix_currency_rates_currency_new_id",
                table: "currency_rates",
                column: "currency_new_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_client_new_id",
                table: "accounts",
                column: "client_new_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_currency_new_id",
                table: "accounts",
                column: "currency_new_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions_details",
                table: "transactions2");

            migrationBuilder.DropIndex(
                name: "ix_transactions2_account_from_new_id",
                table: "transactions2");

            migrationBuilder.DropIndex(
                name: "ix_transactions2_account_to_new_id",
                table: "transactions2");

            migrationBuilder.DropPrimaryKey(
                name: "pk_currency_rates",
                table: "currency_rates");

            migrationBuilder.DropIndex(
                name: "ix_currency_rates_currency_new_id",
                table: "currency_rates");

            migrationBuilder.DropPrimaryKey(
                name: "pk_currencies",
                table: "currencies");

            migrationBuilder.DropPrimaryKey(
                name: "pk_clients",
                table: "clients");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounts",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "ix_accounts_client_new_id",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "ix_accounts_currency_new_id",
                table: "accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_account_balances",
                table: "account_balances");

            migrationBuilder.AlterColumn<int>(
                name: "currency_new_id",
                table: "currency_rates",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions2",
                table: "transactions2",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_currency_rates",
                table: "currency_rates",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_currencies",
                table: "currencies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_clients",
                table: "clients",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounts",
                table: "accounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_account_balances",
                table: "account_balances",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions2_account_from_id",
                table: "transactions2",
                column: "account_from_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions2_account_to_id",
                table: "transactions2",
                column: "account_to_id");

            migrationBuilder.CreateIndex(
                name: "ix_currency_rates_currency_id",
                table: "currency_rates",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_client_id",
                table: "accounts",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_currency_id",
                table: "accounts",
                column: "currency_id");

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
                name: "fk_transactions_details_accounts_account_from_id",
                table: "transactions2",
                column: "account_from_id",
                principalTable: "accounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_details_accounts_account_to_id",
                table: "transactions2",
                column: "account_to_id",
                principalTable: "accounts",
                principalColumn: "id");
        }
    }
}
