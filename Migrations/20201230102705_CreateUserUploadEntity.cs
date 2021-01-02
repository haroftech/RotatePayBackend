using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class CreateUserUploadEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankStatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkIDCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtilityBill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MyProperty0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty9 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUpload_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUpload_UserId",
                table: "UserUpload",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUpload");
        }
    }
}
