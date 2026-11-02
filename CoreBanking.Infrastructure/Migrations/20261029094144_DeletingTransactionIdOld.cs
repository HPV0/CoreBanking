using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletingTransactionIdOld : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            DROP VIEW public.active_transaction_details;

            CREATE VIEW public.active_transaction_details AS
            SELECT
                id,
                transaction_id,
                account_from_id,
                account_to_id,
                exchange_rate,
                amount,
                inserted_at,
                is_active
            FROM public.transactions2
            WHERE is_active = true;
            """);


            migrationBuilder.DropColumn(
                name: "transaction_id_old",
                table: "transactions2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "transaction_id_old",
                table: "transactions2",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("""
            DROP VIEW public.active_transaction_details;

            CREATE VIEW public.active_transaction_details AS
            SELECT
                id,
                transaction_id_old AS transaction_id,
                account_from_id,
                account_to_id,
                exchange_rate,
                amount,
                inserted_at,
                is_active
            FROM public.transactions2
            WHERE is_active = true;
            """);

        }
    }
}
