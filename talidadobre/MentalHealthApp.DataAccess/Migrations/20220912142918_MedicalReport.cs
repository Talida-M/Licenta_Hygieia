using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthApp.DataAccess.Migrations
{
    public partial class MedicalReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PacientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    raportDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    reportDescription = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    medicalHistory = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    condition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    strategy = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    prescription = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    doctorNotes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalReport_SpecialistId",
                        column: x => x.DoctorId,
                        principalTable: "Specialist",
                        principalColumn: "SpecialistId");
                    table.ForeignKey(
                        name: "FK_MedicalReport_Utilizator_PacientId",
                        column: x => x.PacientId,
                        principalTable: "Pacient",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalReport_DoctorId",
                table: "MedicalReport",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalReport_PacientId",
                table: "MedicalReport",
                column: "PacientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalReport");
        }
    }
}
