using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStudios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaId",
                table: "studios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_studios_CinemaId",
                table: "studios",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_studios_cinemas_CinemaId",
                table: "studios",
                column: "CinemaId",
                principalTable: "cinemas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studios_cinemas_CinemaId",
                table: "studios");

            migrationBuilder.DropIndex(
                name: "IX_studios_CinemaId",
                table: "studios");

            migrationBuilder.DropColumn(
                name: "CinemaId",
                table: "studios");
        }
    }
}
