using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AddPaymentNotificationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkStatus",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountPaid = table.Column<double>(type: "float", nullable: false),
                    PaymentChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepositorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PaymentNotifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentNotifications");

            migrationBuilder.DropColumn(
                name: "WorkStatus",
                table: "Users");
        }
    }
}
