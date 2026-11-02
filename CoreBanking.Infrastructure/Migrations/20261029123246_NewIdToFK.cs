using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewIdToFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "account_from_new_id",
                table: "transactions2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "account_to_new_id",
                table: "transactions2",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "currency_new_id",
                table: "currency_rates",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "client_new_id",
                table: "accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "currency_new_id",
                table: "accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE accounts AS a
                SET client_new_id = c.new_id
                FROM clients AS c
                WHERE a.client_id = c.id;

                UPDATE accounts AS a
                SET currency_new_id = c.new_id
                FROM currencies AS c
                WHERE a.currency_id = c.id;

                UPDATE currency_rates AS r
                SET currency_new_id = c.new_id
                FROM currencies AS c
                WHERE r.currency_id = c.id;

                UPDATE transactions2 AS t
                SET account_from_new_id = a.new_id
                FROM accounts AS a
                WHERE t.account_from_id = a.id;

                UPDATE transactions2 AS t
                SET account_to_new_id = a.new_id
                FROM accounts AS a
                WHERE t.account_to_id = a.id;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "account_from_new_id",
                table: "transactions2");

            migrationBuilder.DropColumn(
                name: "account_to_new_id",
                table: "transactions2");

            migrationBuilder.DropColumn(
                name: "currency_new_id",
                table: "currency_rates");

            migrationBuilder.DropColumn(
                name: "client_new_id",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "currency_new_id",
                table: "accounts");
        }
    }
}
