using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoooCart.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchasedPriceProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PurchasedPrice",
                table: "ProductsHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasedPrice",
                table: "ProductsHistories");
        }
    }
}
