using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyStats.DAL.Migrations
{
    public partial class RemoveTransactionCreatedWithRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionCreatedWithRule");

            migrationBuilder.AddColumn<string>(
                name: "AppliedRules",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppliedRules",
                table: "Rule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedRules",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "AppliedRules",
                table: "Rule");

            migrationBuilder.CreateTable(
                name: "TransactionCreatedWithRule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCreatedWithRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionCreatedWithRule_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionCreatedWithRule_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_RuleId",
                table: "TransactionCreatedWithRule",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_TransactionId",
                table: "TransactionCreatedWithRule",
                column: "TransactionId");
        }
    }
}
