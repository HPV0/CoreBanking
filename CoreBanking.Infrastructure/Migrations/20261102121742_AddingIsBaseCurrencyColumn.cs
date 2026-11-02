using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsBaseCurrencyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_base_currency",
                table: "currencies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_is_base_currency",
                table: "currencies",
                column: "is_base_currency",
                unique: true,
                filter: "\"is_base_currency\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_currencies_is_base_currency",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "is_base_currency",
                table: "currencies");
        }
    }
}
