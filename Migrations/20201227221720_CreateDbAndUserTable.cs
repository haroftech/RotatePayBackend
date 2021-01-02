using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class CreateDbAndUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HiDee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lga = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfWorkName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfWorkAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BVN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Integration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubAccountCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileVerified = table.Column<bool>(type: "bit", nullable: false),
                    ContributionLimit = table.Column<double>(type: "float", nullable: false),
                    ContributionLimitRequested = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmationCode = table.Column<int>(type: "int", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmationAttempts = table.Column<int>(type: "int", nullable: false),
                    UserCookie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserCookieChangeCounter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserNumberOfRelatedAccounts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedAccounts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEdited = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
