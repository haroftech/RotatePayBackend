using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class FixUserUploadEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUpload_Users_UserId",
                table: "UserUpload");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUpload",
                table: "UserUpload");

            migrationBuilder.RenameTable(
                name: "UserUpload",
                newName: "UserUploads");

            migrationBuilder.RenameIndex(
                name: "IX_UserUpload_UserId",
                table: "UserUploads",
                newName: "IX_UserUploads_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUploads",
                table: "UserUploads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUploads_Users_UserId",
                table: "UserUploads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUploads_Users_UserId",
                table: "UserUploads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUploads",
                table: "UserUploads");

            migrationBuilder.RenameTable(
                name: "UserUploads",
                newName: "UserUpload");

            migrationBuilder.RenameIndex(
                name: "IX_UserUploads_UserId",
                table: "UserUpload",
                newName: "IX_UserUpload_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUpload",
                table: "UserUpload",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUpload_Users_UserId",
                table: "UserUpload",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
