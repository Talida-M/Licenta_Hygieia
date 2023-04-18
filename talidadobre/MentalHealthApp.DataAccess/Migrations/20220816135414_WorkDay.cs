using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthApp.DataAccess.Migrations
{
    public partial class WorkDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDay", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_WorkDayId",
                table: "DoctorSchedule",
                column: "WorkDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_WorkDay_WorkDayId",
                table: "DoctorSchedule",
                column: "WorkDayId",
                principalTable: "WorkDay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_WorkDay_WorkDayId",
                table: "DoctorSchedule");

            migrationBuilder.DropTable(
                name: "WorkDay");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedule_WorkDayId",
                table: "DoctorSchedule");
        }
    }
}
