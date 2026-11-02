using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceOldTransactionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Assign one integer ID to each distinct old transaction.
            migrationBuilder.Sql("""
        WITH transaction_groups AS (
            SELECT
                transaction_id_old,
                DENSE_RANK() OVER (
                    ORDER BY transaction_id_old
                ) AS new_id
            FROM transactions2
            GROUP BY transaction_id_old
        )
        UPDATE transactions2 AS t
        SET transaction_id = g.new_id
        FROM transaction_groups AS g
        WHERE t.transaction_id_old = g.transaction_id_old;
        """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
