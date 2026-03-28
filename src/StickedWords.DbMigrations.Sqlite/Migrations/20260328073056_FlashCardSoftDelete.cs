using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StickedWords.DbMigrations.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class FlashCardSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "FlashCards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FlashCards",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FlashCards");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FlashCards");
        }
    }
}
