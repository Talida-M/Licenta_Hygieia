using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthApp.DataAccess.Migrations
{
    public partial class DoctorCV0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pageDate",
                table: "UserJournal",
                newName: "PageDate");

            migrationBuilder.RenameColumn(
                name: "isPublic",
                table: "UserJournal",
                newName: "IsPublic");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "UserJournal",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "UserJournal",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800);

            migrationBuilder.CreateTable(
                name: "DoctorCV",
                columns: table => new
                {
                    CVId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataFiles = table.Column<byte[]>(type: "varbinary(MAX)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorCV", x => x.CVId);
                    table.ForeignKey(
                        name: "FK_DoctorCV_Specialist_SpecialistId",
                        column: x => x.SpecialistId,
                        principalTable: "Specialist",
                        principalColumn: "SpecialistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCV_SpecialistId",
                table: "DoctorCV",
                column: "SpecialistId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorCV");

            migrationBuilder.RenameColumn(
                name: "PageDate",
                table: "UserJournal",
                newName: "pageDate");

            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "UserJournal",
                newName: "isPublic");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "UserJournal",
                newName: "content");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "UserJournal",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
