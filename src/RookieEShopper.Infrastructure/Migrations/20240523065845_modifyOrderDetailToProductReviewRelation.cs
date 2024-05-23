using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieEShopper.Migrations
{
    /// <inheritdoc />
    public partial class modifyOrderDetailToProductReviewRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_OrderDetails_OrderDetailId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_OrderDetailId",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "ProductReviews");

            migrationBuilder.AddColumn<int>(
                name: "ProductReviewId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductReviewId",
                table: "OrderDetails",
                column: "ProductReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductReviews_ProductReviewId",
                table: "OrderDetails",
                column: "ProductReviewId",
                principalTable: "ProductReviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductReviews_ProductReviewId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductReviewId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductReviewId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "ProductReviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_OrderDetailId",
                table: "ProductReviews",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_OrderDetails_OrderDetailId",
                table: "ProductReviews",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
