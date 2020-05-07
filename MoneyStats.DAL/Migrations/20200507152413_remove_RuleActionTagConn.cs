using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyStats.DAL.Migrations
{
    public partial class remove_RuleActionTagConn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuleActionTagConn");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "RuleAction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RuleAction_TagId",
                table: "RuleAction",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_RuleAction_Tag_TagId",
                table: "RuleAction",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RuleAction_Tag_TagId",
                table: "RuleAction");

            migrationBuilder.DropIndex(
                name: "IX_RuleAction_TagId",
                table: "RuleAction");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "RuleAction");

            migrationBuilder.CreateTable(
                name: "RuleActionTagConn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RuleActionId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_RuleActionTagConn_RuleActionId",
                table: "RuleActionTagConn",
                column: "RuleActionId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleActionTagConn_TagId",
                table: "RuleActionTagConn",
                column: "TagId");
        }
    }
}
