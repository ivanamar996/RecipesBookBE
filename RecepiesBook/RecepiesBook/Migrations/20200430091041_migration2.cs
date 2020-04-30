using Microsoft.EntityFrameworkCore.Migrations;

namespace RecepiesBook.Migrations
{
    public partial class migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts");

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts",
                column: "RecepieId",
                principalTable: "Recepies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts");

            migrationBuilder.AddForeignKey(
                name: "FK_IngAmounts_Recepies_RecepieId",
                table: "IngAmounts",
                column: "RecepieId",
                principalTable: "Recepies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
