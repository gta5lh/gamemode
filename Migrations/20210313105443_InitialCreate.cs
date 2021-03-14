using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gamemode.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin_rank",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_rank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "varchar(254)", nullable: false),
                    name = table.Column<string>(type: "varchar(32)", nullable: false),
                    password = table.Column<string>(type: "char(60)", nullable: false),
                    admin_rank_id = table.Column<byte>(type: "smallint", nullable: true),
                    muted_by_id = table.Column<long>(type: "bigint", nullable: true),
                    muted_until = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    muted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    mute_reason = table.Column<string>(type: "text", nullable: true),
                    banned_by_id = table.Column<long>(type: "bigint", nullable: true),
                    banned_until = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    banned_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ban_reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_admin_rank_admin_rank_id",
                        column: x => x.admin_rank_id,
                        principalTable: "admin_rank",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_users_banned_by_id",
                        column: x => x.banned_by_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_users_muted_by_id",
                        column: x => x.muted_by_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "weapon",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hash = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapon", x => x.id);
                    table.ForeignKey(
                        name: "FK_weapon_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "admin_rank",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { (byte)1, "Junior" },
                    { (byte)2, "Middle" },
                    { (byte)3, "Senior" },
                    { (byte)4, "Lead" },
                    { (byte)5, "Owner" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_admin_rank_id",
                table: "users",
                column: "admin_rank_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_banned_by_id",
                table: "users",
                column: "banned_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_muted_by_id",
                table: "users",
                column: "muted_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_name",
                table: "users",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weapon_user_id_hash",
                table: "weapon",
                columns: new[] { "user_id", "hash" },
                unique: true);

            migrationBuilder.Sql("SELECT audit.audit_table('users')");
            migrationBuilder.Sql("SELECT audit.audit_table('weapon')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weapon");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "admin_rank");

            migrationBuilder.Sql("DROP TRIGGER audit_trigger_row on users; DROP TRIGGER audit_trigger_stm on users;");
            migrationBuilder.Sql("DROP TRIGGER audit_trigger_row on weapon; DROP TRIGGER audit_trigger_stm on weapon;");
        }
    }
}
