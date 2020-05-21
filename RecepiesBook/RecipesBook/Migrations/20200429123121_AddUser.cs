using Microsoft.EntityFrameworkCore.Migrations;

namespace RecepiesBook.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Ingredients_IngredientId",
                table: "IngAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_ShoppingLists_ShoppingListId",
                table: "IngAmounts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ShoppingLists",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recepies",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Recepies",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Recepies",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingListId",
                table: "IngAmounts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecepieId",
                table: "IngAmounts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "IngAmounts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_Ingredients_IngredientId",
                table: "IngAmounts",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts",
                column: "RecepieId",
                principalTable: "Recepies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_ShoppingLists_ShoppingListId",
                table: "IngAmounts",
                column: "ShoppingListId",
                principalTable: "ShoppingLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Ingredients_IngredientId",
                table: "IngAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_ShoppingLists_ShoppingListId",
                table: "IngAmounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Recepies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ShoppingLists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recepies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Recepies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingListId",
                table: "IngAmounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RecepieId",
                table: "IngAmounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "IngAmounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_Ingredients_IngredientId",
                table: "IngAmounts",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts",
                column: "RecepieId",
                principalTable: "Recepies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_ShoppingLists_ShoppingListId",
                table: "IngAmounts",
                column: "ShoppingListId",
                principalTable: "ShoppingLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
