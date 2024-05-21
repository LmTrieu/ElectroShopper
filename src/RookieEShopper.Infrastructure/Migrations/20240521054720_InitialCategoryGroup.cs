using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieEShopper.Migrations
{
    /// <inheritdoc />
    public partial class InitialCategoryGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryGroupId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryGroupId",
                table: "Categories",
                column: "CategoryGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_CategoryGroups_CategoryGroupId",
                table: "Categories",
                column: "CategoryGroupId",
                principalTable: "CategoryGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_CategoryGroups_CategoryGroupId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryGroups");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryGroupId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryGroupId",
                table: "Categories");
        }
    }
}
