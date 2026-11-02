using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateActiveEntityViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            CREATE VIEW "active_accounts" AS
            SELECT *
            FROM "accounts"
            WHERE "is_active" = TRUE;
            """);

            migrationBuilder.Sql("""
            CREATE VIEW "active_transactions" AS
            SELECT *
            FROM "transactions"
            WHERE "is_active" = TRUE;
            """);

            migrationBuilder.Sql("""
            CREATE VIEW "active_transaction_details" AS
            SELECT *
            FROM "transactions_details"
            WHERE "is_active" = TRUE;
            """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """DROP VIEW IF EXISTS "active_transaction_details";""");

            migrationBuilder.Sql(
                """DROP VIEW IF EXISTS "active_transactions";""");

            migrationBuilder.Sql(
                """DROP VIEW IF EXISTS "active_accounts";""");
        }

    }
}
