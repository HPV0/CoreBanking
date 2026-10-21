using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseSnakeCaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountBalances_Accounts_Id",
                table: "AccountBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Clients_ClientId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Currencies_CurrencyId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_Currencies_CurrencyId",
                table: "CurrencyRates");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsDetails_Accounts_AccountFromId",
                table: "TransactionsDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsDetails_Accounts_AccountToId",
                table: "TransactionsDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsDetails_Transactions_TransactionId",
                table: "TransactionsDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionsDetails",
                table: "TransactionsDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyRates",
                table: "CurrencyRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountBalances",
                table: "AccountBalances");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "transactions");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "currencies");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "clients");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "accounts");

            migrationBuilder.RenameTable(
                name: "TransactionsDetails",
                newName: "transactions_details");

            migrationBuilder.RenameTable(
                name: "CurrencyRates",
                newName: "currency_rates");

            migrationBuilder.RenameTable(
                name: "AccountBalances",
                newName: "account_balances");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "transactions",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "transactions",
                newName: "inserted_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "currencies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "currencies",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "currencies",
                newName: "inserted_at");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "currencies",
                newName: "currency_code");

            migrationBuilder.RenameIndex(
                name: "IX_Currencies_CurrencyCode",
                table: "currencies",
                newName: "ix_currencies_currency_code");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "clients",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "clients",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "clients",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "clients",
                newName: "birthday");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "clients",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PassportNumber",
                table: "clients",
                newName: "passport_number");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "clients",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "clients",
                newName: "inserted_at");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_PassportNumber",
                table: "clients",
                newName: "ix_clients_passport_number");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "accounts",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "accounts",
                newName: "inserted_at");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "accounts",
                newName: "currency_id");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "accounts",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_CurrencyId",
                table: "accounts",
                newName: "ix_accounts_currency_id");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_ClientId",
                table: "accounts",
                newName: "ix_accounts_client_id");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "transactions_details",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transactions_details",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "transactions_details",
                newName: "transaction_id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "transactions_details",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "transactions_details",
                newName: "inserted_at");

            migrationBuilder.RenameColumn(
                name: "ExchangeRate",
                table: "transactions_details",
                newName: "exchange_rate");

            migrationBuilder.RenameColumn(
                name: "AccountToId",
                table: "transactions_details",
                newName: "account_to_id");

            migrationBuilder.RenameColumn(
                name: "AccountFromId",
                table: "transactions_details",
                newName: "account_from_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionsDetails_TransactionId",
                table: "transactions_details",
                newName: "ix_transactions_details_transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionsDetails_AccountToId",
                table: "transactions_details",
                newName: "ix_transactions_details_account_to_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionsDetails_AccountFromId",
                table: "transactions_details",
                newName: "ix_transactions_details_account_from_id");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "currency_rates",
                newName: "rate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "currency_rates",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ValidTo",
                table: "currency_rates",
                newName: "valid_to");

            migrationBuilder.RenameColumn(
                name: "ValidFrom",
                table: "currency_rates",
                newName: "valid_from");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "currency_rates",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "currency_rates",
                newName: "inserted_at");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "currency_rates",
                newName: "currency_id");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRates_CurrencyId",
                table: "currency_rates",
                newName: "ix_currency_rates_currency_id");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "account_balances",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "account_balances",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
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
                name: "pk_transactions_details",
                table: "transactions_details",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_currency_rates",
                table: "currency_rates",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_account_balances",
                table: "account_balances",
                column: "id");

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
                table: "transactions_details",
                column: "account_from_id",
                principalTable: "accounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_details_accounts_account_to_id",
                table: "transactions_details",
                column: "account_to_id",
                principalTable: "accounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_details_transactions_transaction_id",
                table: "transactions_details",
                column: "transaction_id",
                principalTable: "transactions",
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
                name: "fk_transactions_details_accounts_account_from_id",
                table: "transactions_details");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_details_accounts_account_to_id",
                table: "transactions_details");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_details_transactions_transaction_id",
                table: "transactions_details");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_currencies",
                table: "currencies");

            migrationBuilder.DropPrimaryKey(
                name: "pk_clients",
                table: "clients");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounts",
                table: "accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions_details",
                table: "transactions_details");

            migrationBuilder.DropPrimaryKey(
                name: "pk_currency_rates",
                table: "currency_rates");

            migrationBuilder.DropPrimaryKey(
                name: "pk_account_balances",
                table: "account_balances");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "currencies",
                newName: "Currencies");

            migrationBuilder.RenameTable(
                name: "clients",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "Accounts");

            migrationBuilder.RenameTable(
                name: "transactions_details",
                newName: "TransactionsDetails");

            migrationBuilder.RenameTable(
                name: "currency_rates",
                newName: "CurrencyRates");

            migrationBuilder.RenameTable(
                name: "account_balances",
                newName: "AccountBalances");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Transactions",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "inserted_at",
                table: "Transactions",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Currencies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Currencies",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "inserted_at",
                table: "Currencies",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "currency_code",
                table: "Currencies",
                newName: "CurrencyCode");

            migrationBuilder.RenameIndex(
                name: "ix_currencies_currency_code",
                table: "Currencies",
                newName: "IX_Currencies_CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "Clients",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Clients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Clients",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "birthday",
                table: "Clients",
                newName: "Birthday");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Clients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "passport_number",
                table: "Clients",
                newName: "PassportNumber");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Clients",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "inserted_at",
                table: "Clients",
                newName: "InsertedAt");

            migrationBuilder.RenameIndex(
                name: "ix_clients_passport_number",
                table: "Clients",
                newName: "IX_Clients_PassportNumber");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Accounts",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "inserted_at",
                table: "Accounts",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "currency_id",
                table: "Accounts",
                newName: "CurrencyId");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Accounts",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_currency_id",
                table: "Accounts",
                newName: "IX_Accounts_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_client_id",
                table: "Accounts",
                newName: "IX_Accounts_ClientId");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "TransactionsDetails",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TransactionsDetails",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "TransactionsDetails",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "TransactionsDetails",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "inserted_at",
                table: "TransactionsDetails",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "exchange_rate",
                table: "TransactionsDetails",
                newName: "ExchangeRate");

            migrationBuilder.RenameColumn(
                name: "account_to_id",
                table: "TransactionsDetails",
                newName: "AccountToId");

            migrationBuilder.RenameColumn(
                name: "account_from_id",
                table: "TransactionsDetails",
                newName: "AccountFromId");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_details_transaction_id",
                table: "TransactionsDetails",
                newName: "IX_TransactionsDetails_TransactionId");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_details_account_to_id",
                table: "TransactionsDetails",
                newName: "IX_TransactionsDetails_AccountToId");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_details_account_from_id",
                table: "TransactionsDetails",
                newName: "IX_TransactionsDetails_AccountFromId");

            migrationBuilder.RenameColumn(
                name: "rate",
                table: "CurrencyRates",
                newName: "Rate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CurrencyRates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "valid_to",
                table: "CurrencyRates",
                newName: "ValidTo");

            migrationBuilder.RenameColumn(
                name: "valid_from",
                table: "CurrencyRates",
                newName: "ValidFrom");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "CurrencyRates",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "inserted_at",
                table: "CurrencyRates",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "currency_id",
                table: "CurrencyRates",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "ix_currency_rates_currency_id",
                table: "CurrencyRates",
                newName: "IX_CurrencyRates_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "AccountBalances",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AccountBalances",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionsDetails",
                table: "TransactionsDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyRates",
                table: "CurrencyRates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountBalances",
                table: "AccountBalances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountBalances_Accounts_Id",
                table: "AccountBalances",
                column: "Id",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Clients_ClientId",
                table: "Accounts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Currencies_CurrencyId",
                table: "Accounts",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRates_Currencies_CurrencyId",
                table: "CurrencyRates",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsDetails_Accounts_AccountFromId",
                table: "TransactionsDetails",
                column: "AccountFromId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsDetails_Accounts_AccountToId",
                table: "TransactionsDetails",
                column: "AccountToId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsDetails_Transactions_TransactionId",
                table: "TransactionsDetails",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }
    }
}
