using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class User_RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingridient_Recipe_IdRecipe",
                table: "Ingridient");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_Recipe_IdRecipe",
                table: "Step");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingridient_Recipe_IdRecipe",
                table: "Ingridient",
                column: "IdRecipe",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Recipe_IdRecipe",
                table: "Step",
                column: "IdRecipe",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingridient_Recipe_IdRecipe",
                table: "Ingridient");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_Recipe_IdRecipe",
                table: "Step");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingridient_Recipe_IdRecipe",
                table: "Ingridient",
                column: "IdRecipe",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Recipe_IdRecipe",
                table: "Step",
                column: "IdRecipe",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
