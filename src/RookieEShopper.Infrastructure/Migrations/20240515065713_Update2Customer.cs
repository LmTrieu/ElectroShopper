using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieEShopper.Migrations
{
    /// <inheritdoc />
    public partial class Update2Customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EWallet",
                table: "Customers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EWallet",
                table: "Customers");
        }
    }
}