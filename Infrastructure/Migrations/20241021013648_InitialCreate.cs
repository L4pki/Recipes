using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Name = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Tag", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Login = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false ),
                    HashPassword = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false ),
                    Name = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false ),
                    About = table.Column<string>( type: "nvarchar(200)", maxLength: 200, nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_User", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Name = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false ),
                    ShortDescription = table.Column<string>( type: "nvarchar(200)", maxLength: 200, nullable: false ),
                    PhotoURL = table.Column<string>( type: "nvarchar(max)", nullable: false ),
                    IdAuthor = table.Column<int>( type: "int", nullable: false ),
                    TimeCosts = table.Column<TimeSpan>( type: "time", nullable: false ),
                    NumberOfPersons = table.Column<int>( type: "int", nullable: false ),
                    Likes = table.Column<int>( type: "int", nullable: false ),
                    Stars = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Recipe", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Recipe_User_IdAuthor",
                        column: x => x.IdAuthor,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateTable(
                name: "Ingridient",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Title = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false ),
                    Description = table.Column<string>( type: "nvarchar(200)", maxLength: 200, nullable: false ),
                    IdRecipe = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Ingridient", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Ingridient_Recipe_IdRecipe",
                        column: x => x.IdRecipe,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateTable(
                name: "RecipesTagsMapping",
                columns: table => new
                {
                    IdRecipe = table.Column<int>( type: "int", nullable: false ),
                    IdTag = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_RecipesTagsMapping", x => new { x.IdRecipe, x.IdTag } );
                    table.ForeignKey(
                        name: "FK_RecipesTagsMapping_Recipe_IdRecipe",
                        column: x => x.IdRecipe,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                    table.ForeignKey(
                        name: "FK_RecipesTagsMapping_Tag_IdTag",
                        column: x => x.IdTag,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Description = table.Column<string>( type: "nvarchar(200)", maxLength: 200, nullable: false ),
                    IdRecipe = table.Column<int>( type: "int", nullable: false ),
                    NumberOfStep = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Step", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Step_Recipe_IdRecipe",
                        column: x => x.IdRecipe,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateTable(
                name: "UserRecipeLikeMapping",
                columns: table => new
                {
                    IdUser = table.Column<int>( type: "int", nullable: false ),
                    IdRecipe = table.Column<int>( type: "int", nullable: false )
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
                    IdUser = table.Column<int>( type: "int", nullable: false ),
                    IdRecipe = table.Column<int>( type: "int", nullable: false )
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
                name: "IX_Ingridient_IdRecipe",
                table: "Ingridient",
                column: "IdRecipe" );

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_IdAuthor",
                table: "Recipe",
                column: "IdAuthor" );

            migrationBuilder.CreateIndex(
                name: "IX_RecipesTagsMapping_IdTag",
                table: "RecipesTagsMapping",
                column: "IdTag" );

            migrationBuilder.CreateIndex(
                name: "IX_Step_IdRecipe",
                table: "Step",
                column: "IdRecipe" );

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipeLikeMapping_IdUser",
                table: "UserRecipeLikeMapping",
                column: "IdUser" );

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipeStarMapping_IdUser",
                table: "UserRecipeStarMapping",
                column: "IdUser" );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "Ingridient" );

            migrationBuilder.DropTable(
                name: "RecipesTagsMapping" );

            migrationBuilder.DropTable(
                name: "Step" );

            migrationBuilder.DropTable(
                name: "UserRecipeLikeMapping" );

            migrationBuilder.DropTable(
                name: "UserRecipeStarMapping" );

            migrationBuilder.DropTable(
                name: "Tag" );

            migrationBuilder.DropTable(
                name: "Recipe" );

            migrationBuilder.DropTable(
                name: "User" );
        }
    }
}
