using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomWebBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedFieldToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Project",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Project");
        }
    }
}
