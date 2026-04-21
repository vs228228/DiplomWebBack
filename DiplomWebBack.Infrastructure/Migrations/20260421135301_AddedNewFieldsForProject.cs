using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomWebBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewFieldsForProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "Project",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TechnicalTask",
                table: "Project",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TechnicalTask",
                table: "Project");
        }
    }
}
