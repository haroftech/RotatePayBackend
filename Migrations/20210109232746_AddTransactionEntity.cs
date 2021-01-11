using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AddTransactionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountPaid = table.Column<double>(type: "float", nullable: false),
                    PaymentChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
