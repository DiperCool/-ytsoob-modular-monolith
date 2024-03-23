using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ytsoob.Modules.Posts.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "posts");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "reaction_stats",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    like = table.Column<long>(type: "bigint", nullable: false),
                    dislike = table.Column<long>(type: "bigint", nullable: false),
                    angry = table.Column<long>(type: "bigint", nullable: false),
                    wonder = table.Column<long>(type: "bigint", nullable: false),
                    crying = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reaction_stats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ytsoobers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identityid = table.Column<Guid>(name: "identity_id", type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ytsoobers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    commentcontent = table.Column<string>(name: "comment_content", type: "text", nullable: false),
                    reactionstatsid = table.Column<long>(name: "reaction_stats_id", type: "bigint", nullable: false),
                    files = table.Column<List<string>>(type: "text[]", nullable: false),
                    postid = table.Column<long>(name: "post_id", type: "bigint", nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    commentid = table.Column<long>(name: "comment_id", type: "bigint", nullable: true),
                    repliedtocommentid = table.Column<long>(name: "replied_to_comment_id", type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_base_comments_base_comments_comment_id",
                        column: x => x.commentid,
                        principalSchema: "posts",
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_base_comments_base_comments_replied_to_comment_id",
                        column: x => x.repliedtocommentid,
                        principalSchema: "posts",
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_reaction_stats_reaction_stats_id",
                        column: x => x.reactionstatsid,
                        principalSchema: "posts",
                        principalTable: "reaction_stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reactions",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    reactiontype = table.Column<int>(name: "reaction_type", type: "integer", nullable: false),
                    entityid = table.Column<string>(name: "entity_id", type: "text", nullable: false),
                    entitytype = table.Column<string>(name: "entity_type", type: "text", nullable: false),
                    ytsooberid = table.Column<long>(name: "ytsoober_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_reactions_ytsoobers_ytsoober_id",
                        column: x => x.ytsooberid,
                        principalTable: "ytsoobers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    photo = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    ytsooberid = table.Column<long>(name: "ytsoober_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_subscriptions_ytsoobers_ytsoober_id",
                        column: x => x.ytsooberid,
                        principalTable: "ytsoobers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    reactionstatsid = table.Column<long>(name: "reaction_stats_id", type: "bigint", nullable: false),
                    subscriptionid = table.Column<long>(name: "subscription_id", type: "bigint", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_posts_reaction_stats_reaction_stats_id",
                        column: x => x.reactionstatsid,
                        principalSchema: "posts",
                        principalTable: "reaction_stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_posts_subscriptions_subscription_id",
                        column: x => x.subscriptionid,
                        principalTable: "subscriptions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "contents",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    contenttext = table.Column<string>(name: "content_text", type: "character varying(50)", maxLength: 50, nullable: false),
                    postid = table.Column<long>(name: "post_id", type: "bigint", nullable: false),
                    files = table.Column<List<string>>(type: "text[]", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_contents_posts_post_id",
                        column: x => x.postid,
                        principalSchema: "posts",
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "polls",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    postid = table.Column<long>(name: "post_id", type: "bigint", nullable: false),
                    question = table.Column<string>(type: "text", nullable: false),
                    totalcountpoll = table.Column<long>(name: "total_count_poll", type: "bigint", nullable: false),
                    pollanswertype = table.Column<string>(name: "poll_answer_type", type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_polls", x => x.id);
                    table.ForeignKey(
                        name: "fk_polls_posts_post_id",
                        column: x => x.postid,
                        principalSchema: "posts",
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "options",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    optiontitle = table.Column<string>(name: "option_title", type: "character varying(50)", maxLength: 50, nullable: false),
                    optioncount = table.Column<long>(name: "option_count", type: "bigint", nullable: false),
                    fiction = table.Column<decimal>(type: "numeric", nullable: false),
                    pollid = table.Column<long>(name: "poll_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_options_polls_poll_id",
                        column: x => x.pollid,
                        principalSchema: "posts",
                        principalTable: "polls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "voters",
                schema: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    ytsooberid = table.Column<long>(name: "ytsoober_id", type: "bigint", nullable: false),
                    optionid = table.Column<long>(name: "option_id", type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_voters", x => x.id);
                    table.ForeignKey(
                        name: "fk_voters_options_option_id",
                        column: x => x.optionid,
                        principalSchema: "posts",
                        principalTable: "options",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_voters_ytsoobers_ytsoober_id",
                        column: x => x.ytsooberid,
                        principalTable: "ytsoobers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_base_comments_comment_id",
                schema: "posts",
                table: "comments",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_base_comments_replied_to_comment_id",
                schema: "posts",
                table: "comments",
                column: "replied_to_comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_id",
                schema: "posts",
                table: "comments",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_comments_reaction_stats_id",
                schema: "posts",
                table: "comments",
                column: "reaction_stats_id");

            migrationBuilder.CreateIndex(
                name: "ix_contents_id",
                schema: "posts",
                table: "contents",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contents_post_id",
                schema: "posts",
                table: "contents",
                column: "post_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_options_id",
                schema: "posts",
                table: "options",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_options_poll_id",
                schema: "posts",
                table: "options",
                column: "poll_id");

            migrationBuilder.CreateIndex(
                name: "ix_polls_id",
                schema: "posts",
                table: "polls",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_polls_post_id",
                schema: "posts",
                table: "polls",
                column: "post_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_posts_id",
                schema: "posts",
                table: "posts",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_posts_reaction_stats_id",
                schema: "posts",
                table: "posts",
                column: "reaction_stats_id");

            migrationBuilder.CreateIndex(
                name: "ix_posts_subscription_id",
                schema: "posts",
                table: "posts",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "ix_reaction_stats_id",
                schema: "posts",
                table: "reaction_stats",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reactions_id",
                schema: "posts",
                table: "reactions",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reactions_ytsoober_id",
                schema: "posts",
                table: "reactions",
                column: "ytsoober_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_ytsoober_id",
                table: "subscriptions",
                column: "ytsoober_id");

            migrationBuilder.CreateIndex(
                name: "ix_voters_id",
                schema: "posts",
                table: "voters",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_voters_option_id",
                schema: "posts",
                table: "voters",
                column: "option_id");

            migrationBuilder.CreateIndex(
                name: "ix_voters_ytsoober_id",
                schema: "posts",
                table: "voters",
                column: "ytsoober_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "contents",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "reactions",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "voters",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "options",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "polls",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "posts",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "reaction_stats",
                schema: "posts");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "ytsoobers");
        }
    }
}
