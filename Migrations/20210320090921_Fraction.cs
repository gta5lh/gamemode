using Microsoft.EntityFrameworkCore.Migrations;

namespace Gamemode.Migrations
{
    public partial class Fraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "fraction_id",
                table: "users",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "fraction_rank_id",
                table: "users",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "fraction",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fraction", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fraction_rank",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    tier = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "varchar(20)", nullable: false),
                    fraction_id = table.Column<byte>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fraction_rank", x => x.id);
                    table.ForeignKey(
                        name: "FK_fraction_rank_fraction_fraction_id",
                        column: x => x.fraction_id,
                        principalTable: "fraction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "fraction",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { (byte)1, "Bloods" },
                    { (byte)2, "Ballas" },
                    { (byte)3, "The Families" },
                    { (byte)4, "Vagos" },
                    { (byte)5, "Marabunta Grande" }
                });

            migrationBuilder.InsertData(
                table: "fraction_rank",
                columns: new[] { "id", "fraction_id", "name", "tier" },
                values: new object[,]
                {
                    { (byte)1, (byte)1, "Bloods1", (byte)1 },
                    { (byte)28, (byte)3, "Bloods18", (byte)8 },
                    { (byte)29, (byte)3, "Bloods19", (byte)9 },
                    { (byte)30, (byte)3, "Bloods20", (byte)10 },
                    { (byte)31, (byte)4, "Bloods21", (byte)1 },
                    { (byte)32, (byte)4, "Bloods22", (byte)2 },
                    { (byte)33, (byte)4, "Bloods23", (byte)3 },
                    { (byte)34, (byte)4, "Bloods24", (byte)4 },
                    { (byte)35, (byte)4, "Bloods25", (byte)5 },
                    { (byte)36, (byte)4, "Bloods26", (byte)6 },
                    { (byte)37, (byte)4, "Bloods27", (byte)7 },
                    { (byte)38, (byte)4, "Bloods28", (byte)8 },
                    { (byte)39, (byte)4, "Bloods29", (byte)9 },
                    { (byte)40, (byte)4, "Bloods30", (byte)10 },
                    { (byte)41, (byte)5, "Bloods31", (byte)1 },
                    { (byte)42, (byte)5, "Bloods32", (byte)2 },
                    { (byte)43, (byte)5, "Bloods33", (byte)3 },
                    { (byte)44, (byte)5, "Bloods34", (byte)4 },
                    { (byte)45, (byte)5, "Bloods35", (byte)5 },
                    { (byte)46, (byte)5, "Bloods36", (byte)6 },
                    { (byte)47, (byte)5, "Bloods37", (byte)7 },
                    { (byte)48, (byte)5, "Bloods38", (byte)8 },
                    { (byte)27, (byte)3, "Bloods17", (byte)7 },
                    { (byte)26, (byte)3, "Bloods16", (byte)6 },
                    { (byte)25, (byte)3, "Bloods15", (byte)5 },
                    { (byte)24, (byte)3, "Bloods14", (byte)4 },
                    { (byte)2, (byte)1, "Bloods2", (byte)2 },
                    { (byte)3, (byte)1, "Bloods3", (byte)3 },
                    { (byte)4, (byte)1, "Bloods4", (byte)4 },
                    { (byte)5, (byte)1, "Bloods5", (byte)5 },
                    { (byte)6, (byte)1, "Bloods6", (byte)6 },
                    { (byte)7, (byte)1, "Bloods7", (byte)7 },
                    { (byte)8, (byte)1, "Bloods8", (byte)8 },
                    { (byte)9, (byte)1, "Bloods9", (byte)9 },
                    { (byte)10, (byte)1, "Bloods10", (byte)10 },
                    { (byte)11, (byte)2, "Блайд", (byte)1 },
                    { (byte)49, (byte)5, "Bloods39", (byte)9 },
                    { (byte)12, (byte)2, "Бастер", (byte)2 },
                    { (byte)14, (byte)2, "Гун бро", (byte)4 },
                    { (byte)15, (byte)2, "Ап бро", (byte)5 },
                    { (byte)16, (byte)2, "Гангстер", (byte)6 },
                    { (byte)17, (byte)2, "Федерал блок", (byte)7 },
                    { (byte)18, (byte)2, "Фолкс", (byte)8 },
                    { (byte)19, (byte)2, "Райч нига", (byte)9 },
                    { (byte)20, (byte)2, "Биг вилли", (byte)10 },
                    { (byte)21, (byte)3, "Bloods11", (byte)1 },
                    { (byte)22, (byte)3, "Bloods12", (byte)2 },
                    { (byte)23, (byte)3, "Bloods13", (byte)3 },
                    { (byte)13, (byte)2, "Крэкер", (byte)3 },
                    { (byte)50, (byte)5, "Bloods40", (byte)10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_fraction_id",
                table: "users",
                column: "fraction_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_fraction_rank_id",
                table: "users",
                column: "fraction_rank_id");

            migrationBuilder.CreateIndex(
                name: "IX_fraction_rank_fraction_id",
                table: "fraction_rank",
                column: "fraction_id");

            migrationBuilder.CreateIndex(
                name: "IX_fraction_rank_id_name",
                table: "fraction_rank",
                columns: new[] { "id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fraction_rank_id_tier",
                table: "fraction_rank",
                columns: new[] { "id", "tier" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_fraction_fraction_id",
                table: "users",
                column: "fraction_id",
                principalTable: "fraction",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_fraction_rank_fraction_rank_id",
                table: "users",
                column: "fraction_rank_id",
                principalTable: "fraction_rank",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_fraction_fraction_id",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_fraction_rank_fraction_rank_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "fraction_rank");

            migrationBuilder.DropTable(
                name: "fraction");

            migrationBuilder.DropIndex(
                name: "IX_users_fraction_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_fraction_rank_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "fraction_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "fraction_rank_id",
                table: "users");
        }
    }
}
