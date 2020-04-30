using Microsoft.EntityFrameworkCore.Migrations;

namespace RecepiesBook.Migrations
{
    public partial class EditIngAmountModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_ShoppingLists_ShoppingListId",
                table: "IngAmounts");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingListId",
                table: "IngAmounts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RecepieId",
                table: "IngAmounts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_ShoppingLists_ShoppingListId",
                table: "IngAmounts");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingListId",
                table: "IngAmounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecepieId",
                table: "IngAmounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
    }
}
