using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestManagment.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchedulingRelationShipWithTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TestsScheduling_TestId",
                table: "TestsScheduling",
                column: "TestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestsScheduling_TestId",
                table: "TestsScheduling");
        }
    }
}
