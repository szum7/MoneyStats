using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyStats.DAL.Migrations
{
    public partial class UseEnums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rule_RuleType_RuleTypeId",
                table: "Rule");

            migrationBuilder.DropForeignKey(
                name: "FK_RuleAction_RuleActionType_RuleActionTypeId",
                table: "RuleAction");

            migrationBuilder.DropTable(
                name: "RuleActionType");

            migrationBuilder.DropTable(
                name: "RuleType");

            migrationBuilder.DropIndex(
                name: "IX_RuleAction_RuleActionTypeId",
                table: "RuleAction");

            migrationBuilder.DropIndex(
                name: "IX_Rule_RuleTypeId",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "RuleActionTypeId",
                table: "RuleAction");

            migrationBuilder.DropColumn(
                name: "RuleTypeId",
                table: "Rule");

            migrationBuilder.AddColumn<int>(
                name: "RuleActionType",
                table: "RuleAction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuleType",
                table: "Rule",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RuleActionType",
                table: "RuleAction");

            migrationBuilder.DropColumn(
                name: "RuleType",
                table: "Rule");

            migrationBuilder.AddColumn<int>(
                name: "RuleActionTypeId",
                table: "RuleAction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuleTypeId",
                table: "Rule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RuleActionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleActionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RuleAction_RuleActionTypeId",
                table: "RuleAction",
                column: "RuleActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_RuleTypeId",
                table: "Rule",
                column: "RuleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_RuleType_RuleTypeId",
                table: "Rule",
                column: "RuleTypeId",
                principalTable: "RuleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleAction_RuleActionType_RuleActionTypeId",
                table: "RuleAction",
                column: "RuleActionTypeId",
                principalTable: "RuleActionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
