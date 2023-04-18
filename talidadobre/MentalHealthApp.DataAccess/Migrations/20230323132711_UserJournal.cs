using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthApp.DataAccess.Migrations
{
    public partial class UserJournal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "UserJournal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PacientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJournal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserJournal_PacientId",
                        column: x => x.PacientId,
                        principalTable: "Pacient",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

 

            migrationBuilder.CreateIndex(
                name: "IX_UserJournal_PacientId",
                table: "UserJournal",
                column: "PacientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "UserJournal");
        }
    }
}
