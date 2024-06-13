using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupDetail_GroupDetailsGroupDetailId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "GroupDetailsGroupDetailId",
                table: "Groups",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_GroupDetailsGroupDetailId",
                table: "Groups",
                newName: "IX_Groups_Id");

            migrationBuilder.RenameColumn(
                name: "GroupDetailId",
                table: "GroupDetail",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupDetail_Id",
                table: "Groups",
                column: "Id",
                principalTable: "GroupDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupDetail_Id",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Groups",
                newName: "GroupDetailsGroupDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_Id",
                table: "Groups",
                newName: "IX_Groups_GroupDetailsGroupDetailId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GroupDetail",
                newName: "GroupDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupDetail_GroupDetailsGroupDetailId",
                table: "Groups",
                column: "GroupDetailsGroupDetailId",
                principalTable: "GroupDetail",
                principalColumn: "GroupDetailId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
