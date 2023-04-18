using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthApp.DataAccess.Migrations
{
    public partial class Valabilitate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    County = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StreetNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApartmentBuildingFloor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Denumire = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Simptome",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Denumire = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simptome", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumberCountryPrefix = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfFailLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AdresaId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdresaId",
                        column: x => x.AdresaId,
                        principalTable: "Address",
                        principalColumn: "Id");
                }); ;

            migrationBuilder.CreateTable(
                name: "SimptomeDiagnostic",
                columns: table => new
                {
                    DiagnosticId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SimptomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimptomeDiagnostic", x => new { x.DiagnosticId, x.SimptomeId });
                    table.ForeignKey(
                        name: "FK_DiagnosticSimptome_Diagnostic_DiagnosticId",
                        column: x => x.DiagnosticId,
                        principalTable: "Diagnostic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiagnosticSimptome_Simptome_SimptomeId",
                        column: x => x.SimptomeId,
                        principalTable: "Simptome",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discussion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discutie_User_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserIdentityRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserIdentityRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_IdentityUserIdentityRole_IdentityRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "IdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityUserIdentityRole_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenValue = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    RefreshTokenValue = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsTokenRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserToken_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserTokenConfirmation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfirmationTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    ConfirmationToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserTokenConfirmation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserTokenConfirmation_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialist",
                columns: table => new
                {
                    SpecialistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialist", x => x.SpecialistId);
                    table.ForeignKey(
                        name: "FK_SpecialistId",
                        column: x => x.SpecialistId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pacient",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SocialCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HasInsurance = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacient", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscussionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentariiDiscutie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComentariiDiscutie_Discutie_DiscussionId",
                        column: x => x.DiscussionId,
                        principalTable: "Discussion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentariiDiscutie_User_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DoctorSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkDayId = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    End = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSchedule_Specialist_SpecialistId",
                        column: x => x.SpecialistId,
                        principalTable: "Specialist",
                        principalColumn: "SpecialistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CameraConferinta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "date", nullable: false),
                    OraInceput = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OraSfarsit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraConferinta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialistIdConf",
                        column: x => x.SpecialistId,
                        principalTable: "Specialist",
                        principalColumn: "SpecialistId");
                    table.ForeignKey(
                        name: "FK_UtilizatorIdConf",
                        column: x => x.UserId,
                        principalTable: "Pacient",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "IstoricDiagnosticUtilizator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiagnosticId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataDiagnosticare = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IstoricDiagnosticUtilizator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IstoricDiagnosticUtilizator_Diagnostic_DiagnosticId",
                        column: x => x.DiagnosticId,
                        principalTable: "Diagnostic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IstoricDiagnosticUtilizator_Utilizator_UtilizatorId",
                        column: x => x.UserId,
                        principalTable: "Pacient",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppointmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppointmentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialistIdProg",
                        column: x => x.SpecialistId,
                        principalTable: "Specialist",
                        principalColumn: "SpecialistId");
                    table.ForeignKey(
                        name: "FK_UtilizatorId",
                        column: x => x.UserId,
                        principalTable: "Pacient",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UtilizatorSimptome",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SimptomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizatorSimptome", x => new { x.UserId, x.SimptomeId });
                    table.ForeignKey(
                        name: "FK_UtilizatorSimptome_Simptome_SimptomeId",
                        column: x => x.SimptomeId,
                        principalTable: "Simptome",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UtilizatorSimptome_Utilizator_UserId",
                        column: x => x.UserId,
                        principalTable: "Pacient",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CameraConferinta_SpecialistId",
                table: "CameraConferinta",
                column: "SpecialistId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraConferinta_UtilizatorId",
                table: "CameraConferinta",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentariiDiscutie_DiscussionId",
                table: "DiscussionComment",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentariiDiscutie_UserId",
                table: "DiscussionComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Diagnost__19701A4F23232DD2",
                table: "Diagnostic",
                column: "Denumire",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discutie_UserId",
                table: "Discussion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_AdresaId",
                table: "IdentityUser",
                column: "AdresaId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserIdentityRole_RoleId",
                table: "IdentityUserIdentityRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserToken_UserId",
                table: "IdentityUserToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserTokenConfirmation_UserId",
                table: "IdentityUserTokenConfirmation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricDiagnosticUtilizator_DiagnosticId",
                table: "IstoricDiagnosticUtilizator",
                column: "DiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricDiagnosticUtilizator_UtilizatorId",
                table: "IstoricDiagnosticUtilizator",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Programare_SpecialistId",
                table: "Appointment",
                column: "SpecialistId");

            migrationBuilder.CreateIndex(
                name: "IX_Programare_UtilizatorId",
                table: "Appointment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Simptome__19701A4F05BDFAFB",
                table: "Simptome",
                column: "Denumire",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SimptomeDiagnostic_SimptomeId",
                table: "SimptomeDiagnostic",
                column: "SimptomeId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilizatorSimptome_SimptomeId",
                table: "UtilizatorSimptome",
                column: "SimptomeId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_SpecialistId",
                table: "DoctorSchedule",
                column: "SpecialistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraConferinta");

            migrationBuilder.DropTable(
                name: "DiscussionComment");

            migrationBuilder.DropTable(
                name: "IdentityUserIdentityRole");

            migrationBuilder.DropTable(
                name: "IdentityUserToken");

            migrationBuilder.DropTable(
                name: "IdentityUserTokenConfirmation");

            migrationBuilder.DropTable(
                name: "IstoricDiagnosticUtilizator");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "SimptomeDiagnostic");

            migrationBuilder.DropTable(
                name: "UtilizatorSimptome");

            migrationBuilder.DropTable(
                name: "DoctorSchedule");

            migrationBuilder.DropTable(
                name: "Discussion");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropTable(
                name: "Diagnostic");

            migrationBuilder.DropTable(
                name: "Simptome");

            migrationBuilder.DropTable(
                name: "Pacient");

            migrationBuilder.DropTable(
                name: "Specialist");

            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
