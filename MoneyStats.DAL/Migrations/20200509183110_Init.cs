using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyStats.DAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankRow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    IsTransactionCreated = table.Column<bool>(nullable: false),
                    GroupedTransactionId = table.Column<int>(nullable: true),
                    AccountingDate = table.Column<DateTime>(nullable: true),
                    BankTransactionId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    PartnerAccount = table.Column<string>(nullable: true),
                    PartnerName = table.Column<string>(nullable: true),
                    Sum = table.Column<decimal>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankRow", x => x.Id);
                });

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
                name: "Tag",
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
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
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
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Sum = table.Column<decimal>(nullable: true),
                    IsGroup = table.Column<bool>(nullable: false),
                    IsCustom = table.Column<bool>(nullable: false),
                    BankRowId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_BankRow_BankRowId",
                        column: x => x.BankRowId,
                        principalTable: "BankRow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    RuleGroupId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
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
                    table.ForeignKey(
                        name: "FK_RuleAction_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
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
                name: "TransactionTagConn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTagConn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionTagConn_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionTagConn_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
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
                name: "IX_RuleAction_TagId",
                table: "RuleAction",
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
                name: "IX_Transaction_BankRowId",
                table: "Transaction",
                column: "BankRowId",
                unique: true,
                filter: "[BankRowId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_RuleGroupId",
                table: "TransactionCreatedWithRule",
                column: "RuleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCreatedWithRule_TransactionId",
                table: "TransactionCreatedWithRule",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTagConn_TagId",
                table: "TransactionTagConn",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTagConn_TransactionId",
                table: "TransactionTagConn",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "RuleAction");

            migrationBuilder.DropTable(
                name: "RulesetRuleGroupConn");

            migrationBuilder.DropTable(
                name: "TransactionCreatedWithRule");

            migrationBuilder.DropTable(
                name: "TransactionTagConn");

            migrationBuilder.DropTable(
                name: "AndRuleGroup");

            migrationBuilder.DropTable(
                name: "RuleType");

            migrationBuilder.DropTable(
                name: "RuleActionType");

            migrationBuilder.DropTable(
                name: "Ruleset");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "RuleGroup");

            migrationBuilder.DropTable(
                name: "BankRow");
        }
    }
}
