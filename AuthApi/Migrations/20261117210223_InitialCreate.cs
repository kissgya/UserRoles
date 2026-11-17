using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "'NULL'"),
                    regTime = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "'NULL'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "'NULL'"),
                    email = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true, defaultValueSql: "'NULL'"),
                    password = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'NULL'"),
                    regTime = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "'NULL'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userrole",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "char(36)", nullable: false),
                    roleId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.userId, x.roleId });
                    table.ForeignKey(
                        name: "userrole_ibfk_1",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "userrole_ibfk_2",
                        column: x => x.roleId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "roleId",
                table: "userrole",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "userId",
                table: "userrole",
                columns: new[] { "userId", "roleId" });

            migrationBuilder.CreateIndex(
                name: "name1",
                table: "users",
                columns: new[] { "name", "email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userrole");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
