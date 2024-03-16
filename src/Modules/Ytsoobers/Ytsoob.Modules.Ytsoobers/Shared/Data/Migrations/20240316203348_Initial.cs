using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ytsoob.Modules.Ytsoobers.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ytsoobers");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "profiles",
                schema: "ytsoobers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    firstname = table.Column<string>(name: "first_name", type: "character varying(50)", maxLength: 50, nullable: true),
                    lastname = table.Column<string>(name: "last_name", type: "character varying(50)", maxLength: 50, nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ytsoobers",
                schema: "ytsoobers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    profileid = table.Column<long>(name: "profile_id", type: "bigint", nullable: false),
                    creatingcompleted = table.Column<bool>(name: "creating_completed", type: "boolean", nullable: false),
                    identityid = table.Column<Guid>(name: "identity_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ytsoobers", x => x.id);
                    table.ForeignKey(
                        name: "fk_ytsoobers_profiles_profile_temp_id",
                        column: x => x.profileid,
                        principalSchema: "ytsoobers",
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_profiles_id",
                schema: "ytsoobers",
                table: "profiles",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ytsoobers_id",
                schema: "ytsoobers",
                table: "ytsoobers",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ytsoobers_profile_id",
                schema: "ytsoobers",
                table: "ytsoobers",
                column: "profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ytsoobers",
                schema: "ytsoobers");

            migrationBuilder.DropTable(
                name: "profiles",
                schema: "ytsoobers");
        }
    }
}
