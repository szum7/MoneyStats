using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyStats.DAL.Migrations
{
    public partial class RuleModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_BankRow_BankTransactionId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BankTransactionId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BankTransactionId",
                table: "Transaction");

            migrationBuilder.AlterColumn<decimal>(
                name: "Sum",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "BankRowId",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "BankRow",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankTransactionId",
                table: "BankRow",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransactionCreated",
                table: "BankRow",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RuleActionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleActionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ruleset",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruleset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AndRuleGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    RuleGroupId = table.Column<int>(nullable: false)
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
                name: "RuleAction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    RuleActionTypeId = table.Column<int>(nullable: false),
                    Property = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    RuleGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleAction_RuleActionType_RuleActionTypeId",
                        column: x => x.RuleActionTypeId,
                        principalTable: "RuleActionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RuleAction_RuleGroup_RuleGroupId",
                        column: x => x.RuleGroupId,
                        principalTable: "RuleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCreatedWithRule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    RuleGroupId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCreatedWithRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionCreatedWithRule_RuleGroup_RuleGroupId",
                        column: x => x.RuleGroupId,
                        principalTable: "RuleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionCreatedWithRule_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RulesetRuleGroupConn",
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
                    RuleGroupId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Rule",
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
                    RuleTypeId = table.Column<int>(nullable: false),
                    AndRuleGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rule_AndRuleGroup_AndRuleGroupId",
                        column: x => x.AndRuleGroupId,
                        principalTable: "AndRuleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rule_RuleType_RuleTypeId",
                        column: x => x.RuleTypeId,
                        principalTable: "RuleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleActionTagConn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    RuleActionId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleActionTagConn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleActionTagConn_RuleAction_RuleActionId",
                        column: x => x.RuleActionId,
                        principalTable: "RuleAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RuleActionTagConn_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BankRowId",
                table: "Transaction",
                column: "BankRowId");

            migrationBuilder.CreateIndex(
                name: "IX_BankRow_TransactionId",
                table: "BankRow",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AndRuleGroup_RuleGroupId",
                table: "AndRuleGroup",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_AndRuleGroupId",
                table: "Rule",
                column: "AndRuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_RuleTypeId",
                table: "Rule",
                column: "RuleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleAction_RuleActionTypeId",
                table: "RuleAction",
                column: "RuleActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleAction_RuleGroupId",
                table: "RuleAction",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleActionTagConn_RuleActionId",
                table: "RuleActionTagConn",
                column: "RuleActionId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleActionTagConn_TagId",
                table: "RuleActionTagConn",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesetRuleGroupConn_RuleGroupId",
                table: "RulesetRuleGroupConn",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesetRuleGroupConn_RulesetId",
                table: "RulesetRuleGroupConn",
                column: "RulesetId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_RuleGroupId",
                table: "TransactionCreatedWithRule",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_TransactionId",
                table: "TransactionCreatedWithRule",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankRow_Transaction_TransactionId",
                table: "BankRow",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_BankRow_BankRowId",
                table: "Transaction",
                column: "BankRowId",
                principalTable: "BankRow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankRow_Transaction_TransactionId",
                table: "BankRow");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_BankRow_BankRowId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "RuleActionTagConn");

            migrationBuilder.DropTable(
                name: "RulesetRuleGroupConn");

            migrationBuilder.DropTable(
                name: "TransactionCreatedWithRule");

            migrationBuilder.DropTable(
                name: "AndRuleGroup");

            migrationBuilder.DropTable(
                name: "RuleType");

            migrationBuilder.DropTable(
                name: "RuleAction");

            migrationBuilder.DropTable(
                name: "Ruleset");

            migrationBuilder.DropTable(
                name: "RuleActionType");

            migrationBuilder.DropTable(
                name: "RuleGroup");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BankRowId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_BankRow_TransactionId",
                table: "BankRow");

            migrationBuilder.DropColumn(
                name: "BankRowId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BankTransactionId",
                table: "BankRow");

            migrationBuilder.DropColumn(
                name: "IsTransactionCreated",
                table: "BankRow");

            migrationBuilder.AlterColumn<int>(
                name: "Sum",
                table: "Transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transaction",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankTransactionId",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "BankRow",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BankTransactionId",
                table: "Transaction",
                column: "BankTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_BankRow_BankTransactionId",
                table: "Transaction",
                column: "BankTransactionId",
                principalTable: "BankRow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
