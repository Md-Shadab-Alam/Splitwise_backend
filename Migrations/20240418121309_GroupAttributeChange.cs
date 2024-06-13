using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class GroupAttributeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Balances_BalanceId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_BalanceId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "BalanceId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.AddColumn<int>(
                name: "BalanceId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BalanceId",
                table: "Expenses",
                column: "BalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Balances_BalanceId",
                table: "Expenses",
                column: "BalanceId",
                principalTable: "Balances",
                principalColumn: "BalanceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
