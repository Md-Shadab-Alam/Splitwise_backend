using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupDetail_Id",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Groups",
                newName: "GroupDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_Id",
                table: "Groups",
                newName: "IX_Groups_GroupDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupDetail_GroupDetailsId",
                table: "Groups",
                column: "GroupDetailsId",
                principalTable: "GroupDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupDetail_GroupDetailsId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "GroupDetailsId",
                table: "Groups",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_GroupDetailsId",
                table: "Groups",
                newName: "IX_Groups_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupDetail_Id",
                table: "Groups",
                column: "Id",
                principalTable: "GroupDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
