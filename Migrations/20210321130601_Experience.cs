using Microsoft.EntityFrameworkCore.Migrations;

namespace Gamemode.Migrations
{
    public partial class Experience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "current_experience",
                table: "users",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "required_experience_to_rank_up",
                table: "fraction_rank",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)1,
                column: "required_experience_to_rank_up",
                value: (short)21);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)2,
                column: "required_experience_to_rank_up",
                value: (short)35);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)3,
                column: "required_experience_to_rank_up",
                value: (short)49);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)4,
                column: "required_experience_to_rank_up",
                value: (short)63);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)5,
                column: "required_experience_to_rank_up",
                value: (short)77);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)6,
                column: "required_experience_to_rank_up",
                value: (short)91);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)7,
                column: "required_experience_to_rank_up",
                value: (short)105);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)8,
                column: "required_experience_to_rank_up",
                value: (short)119);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)9,
                column: "required_experience_to_rank_up",
                value: (short)140);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)11,
                column: "required_experience_to_rank_up",
                value: (short)21);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)12,
                column: "required_experience_to_rank_up",
                value: (short)35);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)13,
                column: "required_experience_to_rank_up",
                value: (short)49);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)14,
                column: "required_experience_to_rank_up",
                value: (short)63);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)15,
                column: "required_experience_to_rank_up",
                value: (short)77);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)16,
                column: "required_experience_to_rank_up",
                value: (short)91);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)17,
                column: "required_experience_to_rank_up",
                value: (short)105);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)18,
                column: "required_experience_to_rank_up",
                value: (short)119);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)19,
                column: "required_experience_to_rank_up",
                value: (short)140);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)21,
                column: "required_experience_to_rank_up",
                value: (short)21);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)22,
                column: "required_experience_to_rank_up",
                value: (short)35);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)23,
                column: "required_experience_to_rank_up",
                value: (short)49);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)24,
                column: "required_experience_to_rank_up",
                value: (short)63);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)25,
                column: "required_experience_to_rank_up",
                value: (short)77);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)26,
                column: "required_experience_to_rank_up",
                value: (short)91);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)27,
                column: "required_experience_to_rank_up",
                value: (short)105);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)28,
                column: "required_experience_to_rank_up",
                value: (short)119);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)29,
                column: "required_experience_to_rank_up",
                value: (short)140);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)31,
                column: "required_experience_to_rank_up",
                value: (short)21);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)32,
                column: "required_experience_to_rank_up",
                value: (short)35);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)33,
                column: "required_experience_to_rank_up",
                value: (short)49);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)34,
                column: "required_experience_to_rank_up",
                value: (short)63);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)35,
                column: "required_experience_to_rank_up",
                value: (short)77);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)36,
                column: "required_experience_to_rank_up",
                value: (short)91);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)37,
                column: "required_experience_to_rank_up",
                value: (short)105);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)38,
                column: "required_experience_to_rank_up",
                value: (short)119);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)39,
                column: "required_experience_to_rank_up",
                value: (short)140);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)41,
                column: "required_experience_to_rank_up",
                value: (short)21);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)42,
                column: "required_experience_to_rank_up",
                value: (short)35);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)43,
                column: "required_experience_to_rank_up",
                value: (short)49);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)44,
                column: "required_experience_to_rank_up",
                value: (short)63);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)45,
                column: "required_experience_to_rank_up",
                value: (short)77);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)46,
                column: "required_experience_to_rank_up",
                value: (short)91);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)47,
                column: "required_experience_to_rank_up",
                value: (short)105);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)48,
                column: "required_experience_to_rank_up",
                value: (short)119);

            migrationBuilder.UpdateData(
                table: "fraction_rank",
                keyColumn: "id",
                keyValue: (byte)49,
                column: "required_experience_to_rank_up",
                value: (short)140);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_experience",
                table: "users");

            migrationBuilder.DropColumn(
                name: "required_experience_to_rank_up",
                table: "fraction_rank");
        }
    }
}
