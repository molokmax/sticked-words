using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StickedWords.DbMigrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddImageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "FlashCards",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "FlashCards");
        }
    }
}
