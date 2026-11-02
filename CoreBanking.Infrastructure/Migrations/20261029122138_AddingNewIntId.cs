using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewIntId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "clients",
                type: "integer",
                nullable: false)
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "accounts",
                type: "integer",
                nullable: false)
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "currencies",
                type: "integer",
                nullable: false)
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "currency_rates",
                type: "integer",
                nullable: false)
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "transactions2",
                type: "integer",
                nullable: false)
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            // AccountBalance.NewId must match its corresponding Account.NewId.
            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "account_balances",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql("""
        UPDATE account_balances AS b
        SET new_id = a.new_id
        FROM accounts AS a
        WHERE b.id = a.id;
        """);

            migrationBuilder.AlterColumn<int>(
                name: "new_id",
                table: "account_balances",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "new_id",
                table: "transactions2");

            migrationBuilder.DropColumn(
                name: "new_id",
                table: "currency_rates");

            migrationBuilder.DropColumn(
                name: "new_id",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "new_id",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "new_id",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "new_id",
                table: "account_balances");
        }
    }
}
