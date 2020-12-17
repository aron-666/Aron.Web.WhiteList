using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace intro.Migrations.WhiteList
{
    public partial class whiteLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Whitelists",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Route = table.Column<string>(type: "varchar(60)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Whitelists", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WL_Content",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Wid = table.Column<long>(type: "bigint(20)", nullable: false),
                    Content = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Policy = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Created = table.Column<DateTime>(type: "datetime", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WL_Content", x => x.ID);
                    table.ForeignKey(
                        name: "WL_Content_ibfk_1",
                        column: x => x.Wid,
                        principalTable: "Whitelists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "Name",
                table: "Whitelists",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Route",
                table: "Whitelists",
                column: "Route",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Wid",
                table: "WL_Content",
                columns: new[] { "Wid", "Content" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WL_Content");

            migrationBuilder.DropTable(
                name: "Whitelists");
        }
    }
}
