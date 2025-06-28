using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StickedWords.DbMigrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddLearningSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "FlashCards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LearningSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiringAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionFlashCards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LearningSessionId = table.Column<long>(type: "bigint", nullable: false),
                    FlashCardId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionFlashCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionFlashCards_FlashCards_FlashCardId",
                        column: x => x.FlashCardId,
                        principalTable: "FlashCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionFlashCards_LearningSessions_LearningSessionId",
                        column: x => x.LearningSessionId,
                        principalTable: "LearningSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionStages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdNumber = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ExerciseType = table.Column<int>(type: "integer", nullable: false),
                    LearningSessionId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentFlashCardId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionStages_LearningSessions_LearningSessionId",
                        column: x => x.LearningSessionId,
                        principalTable: "LearningSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionStages_SessionFlashCards_CurrentFlashCardId",
                        column: x => x.CurrentFlashCardId,
                        principalTable: "SessionFlashCards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Guesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionStageId = table.Column<long>(type: "bigint", nullable: false),
                    FlashCardId = table.Column<long>(type: "bigint", nullable: false),
                    Result = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guesses_FlashCards_FlashCardId",
                        column: x => x.FlashCardId,
                        principalTable: "FlashCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Guesses_SessionStages_SessionStageId",
                        column: x => x.SessionStageId,
                        principalTable: "SessionStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guesses_FlashCardId",
                table: "Guesses",
                column: "FlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Guesses_SessionStageId",
                table: "Guesses",
                column: "SessionStageId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionFlashCards_FlashCardId",
                table: "SessionFlashCards",
                column: "FlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionFlashCards_LearningSessionId",
                table: "SessionFlashCards",
                column: "LearningSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionStages_CurrentFlashCardId",
                table: "SessionStages",
                column: "CurrentFlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionStages_LearningSessionId",
                table: "SessionStages",
                column: "LearningSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guesses");

            migrationBuilder.DropTable(
                name: "SessionStages");

            migrationBuilder.DropTable(
                name: "SessionFlashCards");

            migrationBuilder.DropTable(
                name: "LearningSessions");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "FlashCards");
        }
    }
}
