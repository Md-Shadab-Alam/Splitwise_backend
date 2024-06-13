using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_Users_UsersId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UsersId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_UsersId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Balances_UsersId",
                table: "Balances");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupDetailsGroupDetailId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExpenseDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseDetail_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "ExpenseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupDetail",
                columns: table => new
                {
                    GroupDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDetail", x => x.GroupDetailId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExpenseId",
                table: "Users",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupDetailsGroupDetailId",
                table: "Groups",
                column: "GroupDetailsGroupDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseDetail_ExpenseId",
                table: "ExpenseDetail",
                column: "ExpenseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupDetail_GroupDetailsGroupDetailId",
                table: "Groups",
                column: "GroupDetailsGroupDetailId",
                principalTable: "GroupDetail",
                principalColumn: "GroupDetailId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Expenses_ExpenseId",
                table: "Users",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "ExpenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupDetail_GroupDetailsGroupDetailId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Expenses_ExpenseId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ExpenseDetail");

            migrationBuilder.DropTable(
                name: "GroupDetail");

            migrationBuilder.DropIndex(
                name: "IX_Users_ExpenseId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupDetailsGroupDetailId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupDetailsGroupDetailId",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UsersId",
                table: "Expenses",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_UsersId",
                table: "Balances",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_Users_UsersId",
                table: "Balances",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "UsersId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_UsersId",
                table: "Expenses",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "UsersId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
