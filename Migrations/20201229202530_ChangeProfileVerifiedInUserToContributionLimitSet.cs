using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class ChangeProfileVerifiedInUserToContributionLimitSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileVerified",
                table: "Users",
                newName: "ContributionLimitSet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContributionLimitSet",
                table: "Users",
                newName: "ProfileVerified");
        }
    }
}
