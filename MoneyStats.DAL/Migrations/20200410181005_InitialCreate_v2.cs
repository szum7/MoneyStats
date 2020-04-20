using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyStats.DAL.Migrations
{
    public partial class InitialCreate_v2 : Migration
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
                    BankType = table.Column<int>(nullable: false),
                    TransactionGroupId = table.Column<int>(nullable: true),
                    AccountingDate = table.Column<DateTime>(nullable: true),
                    TransactionId = table.Column<string>(nullable: true),
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
                    Date = table.Column<DateTime>(nullable: false),
                    Sum = table.Column<int>(nullable: false),
                    IsGroup = table.Column<bool>(nullable: false, defaultValue: false),
                    IsCustom = table.Column<bool>(nullable: false, defaultValue: false),
                    BankTransactionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_BankRow_BankTransactionId",
                        column: x => x.BankTransactionId,
                        principalTable: "BankRow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BankTransactionId",
                table: "Transaction",
                column: "BankTransactionId");

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
                name: "TransactionTagConn");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "BankRow");
        }
    }
}
