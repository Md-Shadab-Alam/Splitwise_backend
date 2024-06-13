using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class BalanceUsersNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Balances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Balances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
