using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomWebBack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserActivator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Activators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AprovedById = table.Column<Guid>(type: "uuid", nullable: true),
                    AprovedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activators_User_AprovedById",
                        column: x => x.AprovedById,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activators_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activators_AprovedById",
                table: "Activators",
                column: "AprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activators_UserId",
                table: "Activators",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activators");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "User");
        }
    }
}
