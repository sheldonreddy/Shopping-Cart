using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GG_shopping_cart.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToCartCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserSession",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserSession",
                table: "Carts",
                column: "UserSession",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_UserSession",
                table: "Carts");

            migrationBuilder.AlterColumn<string>(
                name: "UserSession",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
