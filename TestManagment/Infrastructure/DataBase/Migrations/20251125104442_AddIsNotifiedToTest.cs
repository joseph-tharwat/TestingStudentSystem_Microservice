using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddIsNotifiedToTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotified",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotified",
                table: "Tests");
        }
    }
}
