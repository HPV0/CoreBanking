using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingTransactionV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Preserve the existing transaction details and rename the table.
            migrationBuilder.RenameTable(
                name: "transactions_details",
                newName: "transactions2");

            // Rename the existing detail ID column if needed.
            // The existing "id" column remains unchanged.

            // Preserve the old grouping ID in a separate column.
            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "transactions2",
                newName: "transaction_id_old");

            // Remove the old FK/index if they exist and are no longer needed.
            migrationBuilder.DropForeignKey(
                name: "fk_transactions_details_transactions_transaction_id",
                table: "transactions2");

            migrationBuilder.DropIndex(
                name: "ix_transactions_details_transaction_id",
                table: "transactions2");

            // Add the new grouping column.
            migrationBuilder.AddColumn<int>(
                name: "transaction_id",
                table: "transactions2",
                type: "integer",
                nullable: true,
                defaultValueSql: "txid_current()");

            // Rename the indexes for the renamed table.
            migrationBuilder.RenameIndex(
                name: "ix_transactions_details_account_from_id",
                table: "transactions2",
                newName: "ix_transactions2_account_from_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_details_account_to_id",
                table: "transactions2",
                newName: "ix_transactions2_account_to_id");

            migrationBuilder.Sql("""
                DROP VIEW public.active_transactions;
                """);

            // Remove the separate parent table after its FK is removed.
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.Sql("""
                CREATE VIEW public.active_transactions AS
                SELECT id, inserted_at, is_active
                FROM transactions2
                WHERE is_active = true;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP VIEW public.active_transactions;
                """);
            // Recreate the original parent table.
            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(
                        type: "uuid",
                        nullable: false),
                    inserted_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"),
                    is_active = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            // Remove the new integer grouping ID.
            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "transactions2");

            // Restore the original UUID transaction_id column.
            migrationBuilder.RenameColumn(
                name: "transaction_id_old",
                table: "transactions2",
                newName: "transaction_id");

            // Restore the original table name.
            migrationBuilder.RenameTable(
                name: "transactions2",
                newName: "transactions_details");

            // Restore index names.
            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_from_id",
                table: "transactions_details",
                newName: "ix_transactions_details_account_from_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions2_account_to_id",
                table: "transactions_details",
                newName: "ix_transactions_details_account_to_id");

            // Restore the index on transaction_id.
            migrationBuilder.CreateIndex(
                name: "ix_transactions_details_transaction_id",
                table: "transactions_details",
                column: "transaction_id");

            // Restore the original foreign key.
            migrationBuilder.AddForeignKey(
                name: "fk_transactions_details_transactions_transaction_id",
                table: "transactions_details",
                column: "transaction_id",
                principalTable: "transactions",
                principalColumn: "id");

            migrationBuilder.Sql("""
    CREATE VIEW public.active_transactions AS
    SELECT id, inserted_at, is_active
    FROM transactions
    WHERE is_active = true;
    """);
        }
    }
}
