using Microsoft.EntityFrameworkCore.Migrations;

namespace WinterClassesApp.Migrations
{
    public partial class AddedUserImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "JobApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "JobApplications");
        }
    }
}
