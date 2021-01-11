using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AddGuarantorEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanGuarantee",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanGuaranteeRequested",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "ContributionAmount",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ContributionAmountLocked",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "GuaranteeLocked",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Guarantors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuarantorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuaranteeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Guarantors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guarantors");

            migrationBuilder.DropColumn(
                name: "CanGuarantee",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CanGuaranteeRequested",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContributionAmount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContributionAmountLocked",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GuaranteeLocked",
                table: "Users");
        }
    }
}
