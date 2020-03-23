using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreProj.Migrations
{
    public partial class addphotopath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "EmployeesDBSet",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "EmployeesDBSet");
        }
    }
}
