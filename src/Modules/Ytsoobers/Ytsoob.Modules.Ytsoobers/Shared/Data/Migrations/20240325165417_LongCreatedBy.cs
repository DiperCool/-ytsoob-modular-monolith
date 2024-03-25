using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ytsoob.Modules.Ytsoobers.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class LongCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ytsoobers_profiles_profile_temp_id",
                schema: "ytsoobers",
                table: "ytsoobers");

            migrationBuilder.AlterColumn<long>(
                name: "created_by",
                schema: "ytsoobers",
                table: "ytsoobers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "created_by",
                schema: "ytsoobers",
                table: "profiles",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_ytsoobers_profiles_profile_id",
                schema: "ytsoobers",
                table: "ytsoobers",
                column: "profile_id",
                principalSchema: "ytsoobers",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ytsoobers_profiles_profile_id",
                schema: "ytsoobers",
                table: "ytsoobers");

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "ytsoobers",
                table: "ytsoobers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "ytsoobers",
                table: "profiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_ytsoobers_profiles_profile_temp_id",
                schema: "ytsoobers",
                table: "ytsoobers",
                column: "profile_id",
                principalSchema: "ytsoobers",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
