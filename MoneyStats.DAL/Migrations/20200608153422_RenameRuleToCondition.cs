using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyStats.DAL.Migrations
{
    public partial class RenameRuleToCondition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rule_AndRuleGroup_AndRuleGroupId",
                table: "Rule");

            migrationBuilder.DropForeignKey(
                name: "FK_RuleAction_RuleGroup_RuleGroupId",
                table: "RuleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCreatedWithRule_RuleGroup_RuleGroupId",
                table: "TransactionCreatedWithRule");

            migrationBuilder.DropTable(
                name: "AndRuleGroup");

            migrationBuilder.DropTable(
                name: "RulesetRuleGroupConn");

            migrationBuilder.DropTable(
                name: "RuleGroup");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCreatedWithRule_RuleGroupId",
                table: "TransactionCreatedWithRule");

            migrationBuilder.DropIndex(
                name: "IX_RuleAction_RuleGroupId",
                table: "RuleAction");

            migrationBuilder.DropIndex(
                name: "IX_Rule_AndRuleGroupId",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "RuleGroupId",
                table: "TransactionCreatedWithRule");

            migrationBuilder.DropColumn(
                name: "RuleGroupId",
                table: "RuleAction");

            migrationBuilder.DropColumn(
                name: "AndRuleGroupId",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "Property",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "RuleType",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Rule");

            migrationBuilder.AddColumn<int>(
                name: "RuleId",
                table: "TransactionCreatedWithRule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuleId",
                table: "RuleAction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Rule",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AndConditionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    RuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AndConditionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AndConditionGroup_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RulesetRuleConn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    RulesetId = table.Column<int>(nullable: false),
                    RuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulesetRuleConn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulesetRuleConn_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RulesetRuleConn_Ruleset_RulesetId",
                        column: x => x.RulesetId,
                        principalTable: "Ruleset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Property = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ConditionType = table.Column<int>(nullable: false),
                    AndConditionGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Condition_AndConditionGroup_AndConditionGroupId",
                        column: x => x.AndConditionGroupId,
                        principalTable: "AndConditionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_RuleId",
                table: "TransactionCreatedWithRule",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleAction_RuleId",
                table: "RuleAction",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AndConditionGroup_RuleId",
                table: "AndConditionGroup",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Condition_AndConditionGroupId",
                table: "Condition",
                column: "AndConditionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesetRuleConn_RuleId",
                table: "RulesetRuleConn",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesetRuleConn_RulesetId",
                table: "RulesetRuleConn",
                column: "RulesetId");

            migrationBuilder.AddForeignKey(
                name: "FK_RuleAction_Rule_RuleId",
                table: "RuleAction",
                column: "RuleId",
                principalTable: "Rule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCreatedWithRule_Rule_RuleId",
                table: "TransactionCreatedWithRule",
                column: "RuleId",
                principalTable: "Rule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RuleAction_Rule_RuleId",
                table: "RuleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCreatedWithRule_Rule_RuleId",
                table: "TransactionCreatedWithRule");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "RulesetRuleConn");

            migrationBuilder.DropTable(
                name: "AndConditionGroup");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCreatedWithRule_RuleId",
                table: "TransactionCreatedWithRule");

            migrationBuilder.DropIndex(
                name: "IX_RuleAction_RuleId",
                table: "RuleAction");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "TransactionCreatedWithRule");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "RuleAction");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Rule");

            migrationBuilder.AddColumn<int>(
                name: "RuleGroupId",
                table: "TransactionCreatedWithRule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuleGroupId",
                table: "RuleAction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AndRuleGroupId",
                table: "Rule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Property",
                table: "Rule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RuleType",
                table: "Rule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Rule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RuleGroup",
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
                    table.PrimaryKey("PK_RuleGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AndRuleGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleGroupId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AndRuleGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AndRuleGroup_RuleGroup_RuleGroupId",
                        column: x => x.RuleGroupId,
                        principalTable: "RuleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RulesetRuleGroupConn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleGroupId = table.Column<int>(type: "int", nullable: false),
                    RulesetId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulesetRuleGroupConn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulesetRuleGroupConn_RuleGroup_RuleGroupId",
                        column: x => x.RuleGroupId,
                        principalTable: "RuleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RulesetRuleGroupConn_Ruleset_RulesetId",
                        column: x => x.RulesetId,
                        principalTable: "Ruleset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_RuleGroupId",
                table: "TransactionCreatedWithRule",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleAction_RuleGroupId",
                table: "RuleAction",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_AndRuleGroupId",
                table: "Rule",
                column: "AndRuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AndRuleGroup_RuleGroupId",
                table: "AndRuleGroup",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesetRuleGroupConn_RuleGroupId",
                table: "RulesetRuleGroupConn",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesetRuleGroupConn_RulesetId",
                table: "RulesetRuleGroupConn",
                column: "RulesetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_AndRuleGroup_AndRuleGroupId",
                table: "Rule",
                column: "AndRuleGroupId",
                principalTable: "AndRuleGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleAction_RuleGroup_RuleGroupId",
                table: "RuleAction",
                column: "RuleGroupId",
                principalTable: "RuleGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCreatedWithRule_RuleGroup_RuleGroupId",
                table: "TransactionCreatedWithRule",
                column: "RuleGroupId",
                principalTable: "RuleGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
