using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomWebBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTagToProjectm2m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "TagsToProjects",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Year",
                table: "TagsToProjects",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "TagsToProjects");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "TagsToProjects");
        }
    }
}
