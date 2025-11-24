using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradingManagment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeStudentIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_StudentGrades", "StudentGrades");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "StudentGrades",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey("PK_StudentGrades", "StudentGrades", "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
