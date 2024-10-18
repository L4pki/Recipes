using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTagsMapping_Recipe_IdRecipe",
                table: "RecipesTagsMapping" );

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTagsMapping_Tag_IdTag",
                table: "RecipesTagsMapping" );

            migrationBuilder.DropTable(
                name: "UserRecipeLikeMapping" );

            migrationBuilder.DropTable(
                name: "UserRecipeStarMapping" );

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Recipe" );

            migrationBuilder.DropColumn(
                name: "Stars",
                table: "Recipe" );

            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "User",
                newName: "PasswordHash" );

            migrationBuilder.RenameColumn(
                name: "IdTag",
                table: "RecipesTagsMapping",
                newName: "TagsId" );

            migrationBuilder.RenameColumn(
                name: "IdRecipe",
                table: "RecipesTagsMapping",
                newName: "RecipesByTagId" );

            migrationBuilder.RenameIndex(
                name: "IX_RecipesTagsMapping_IdTag",
                table: "RecipesTagsMapping",
                newName: "IX_RecipesTagsMapping_TagsId" );

            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "Recipe",
                newName: "PhotoUrl" );

            migrationBuilder.CreateTable(
                name: "UsersRecipesLikeMapping",
                columns: table => new
                {
                    LikeRecipesId = table.Column<int>( type: "int", nullable: false ),
                    UsersLikesId = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_UsersRecipesLikeMapping", x => new { x.LikeRecipesId, x.UsersLikesId } );
                    table.ForeignKey(
                        name: "FK_UsersRecipesLikeMapping_Recipe_LikeRecipesId",
                        column: x => x.LikeRecipesId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                    table.ForeignKey(
                        name: "FK_UsersRecipesLikeMapping_User_UsersLikesId",
                        column: x => x.UsersLikesId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateTable(
                name: "UsersRecipesStarMapping",
                columns: table => new
                {
                    FavoriteRecipesId = table.Column<int>( type: "int", nullable: false ),
                    UsersStarsId = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_UsersRecipesStarMapping", x => new { x.FavoriteRecipesId, x.UsersStarsId } );
                    table.ForeignKey(
                        name: "FK_UsersRecipesStarMapping_Recipe_FavoriteRecipesId",
                        column: x => x.FavoriteRecipesId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                    table.ForeignKey(
                        name: "FK_UsersRecipesStarMapping_User_UsersStarsId",
                        column: x => x.UsersStarsId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_UsersRecipesLikeMapping_UsersLikesId",
                table: "UsersRecipesLikeMapping",
                column: "UsersLikesId" );

            migrationBuilder.CreateIndex(
                name: "IX_UsersRecipesStarMapping_UsersStarsId",
                table: "UsersRecipesStarMapping",
                column: "UsersStarsId" );

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTagsMapping_Recipe_RecipesByTagId",
                table: "RecipesTagsMapping",
                column: "RecipesByTagId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade );

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTagsMapping_Tag_TagsId",
                table: "RecipesTagsMapping",
                column: "TagsId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTagsMapping_Recipe_RecipesByTagId",
                table: "RecipesTagsMapping" );

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTagsMapping_Tag_TagsId",
                table: "RecipesTagsMapping" );

            migrationBuilder.DropTable(
                name: "UsersRecipesLikeMapping" );

            migrationBuilder.DropTable(
                name: "UsersRecipesStarMapping" );

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "User",
                newName: "HashPassword" );

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "RecipesTagsMapping",
                newName: "IdTag" );

            migrationBuilder.RenameColumn(
                name: "RecipesByTagId",
                table: "RecipesTagsMapping",
                newName: "IdRecipe" );

            migrationBuilder.RenameIndex(
                name: "IX_RecipesTagsMapping_TagsId",
                table: "RecipesTagsMapping",
                newName: "IX_RecipesTagsMapping_IdTag" );

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Recipe",
                newName: "PhotoURL" );

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0 );

            migrationBuilder.AddColumn<int>(
                name: "Stars",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0 );

            migrationBuilder.CreateTable(
                name: "UserRecipeLikeMapping",
                columns: table => new
                {
                    IdRecipe = table.Column<int>( type: "int", nullable: false ),
                    IdUser = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_UserRecipeLikeMapping", x => new { x.IdRecipe, x.IdUser } );
                    table.ForeignKey(
                        name: "FK_UserRecipeLikeMapping_Recipe_IdRecipe",
                        column: x => x.IdRecipe,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                    table.ForeignKey(
                        name: "FK_UserRecipeLikeMapping_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateTable(
                name: "UserRecipeStarMapping",
                columns: table => new
                {
                    IdRecipe = table.Column<int>( type: "int", nullable: false ),
                    IdUser = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_UserRecipeStarMapping", x => new { x.IdRecipe, x.IdUser } );
                    table.ForeignKey(
                        name: "FK_UserRecipeStarMapping_Recipe_IdRecipe",
                        column: x => x.IdRecipe,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                    table.ForeignKey(
                        name: "FK_UserRecipeStarMapping_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipeLikeMapping_IdUser",
                table: "UserRecipeLikeMapping",
                column: "IdUser" );

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipeStarMapping_IdUser",
                table: "UserRecipeStarMapping",
                column: "IdUser" );

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTagsMapping_Recipe_IdRecipe",
                table: "RecipesTagsMapping",
                column: "IdRecipe",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict );

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTagsMapping_Tag_IdTag",
                table: "RecipesTagsMapping",
                column: "IdTag",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict );
        }
    }
}
