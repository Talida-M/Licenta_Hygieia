﻿// <auto-generated />
using System;
using MentalHealthApp.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MentalHealthApp.DataAccess.Migrations
{
    [DbContext(typeof(MentalHealthAppContext))]
    [Migration("20220816135414_WorkDay")]
    partial class WorkDay
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("IdentityUserIdentityRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("IdentityUserIdentityRole", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ApartmentBuildingFloor")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(6)
                        .HasColumnType("nchar(6)")
                        .IsFixedLength();

                    b.Property<string>("County")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Sector")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StreetNumber")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.CameraConferintum", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("date");

                    b.Property<string>("OraInceput")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("OraSfarsit")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("SpecialistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SpecialistId");

                    b.HasIndex("UserId");

                    b.ToTable("CameraConferinta");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.DiscussionComment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MessageContent");

                    b.Property<DateTime>("CommentDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CommentDate");

                    b.Property<Guid>("DiscussionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DiscussionId");

                    b.HasIndex("UserId");

                    b.ToTable("DiscussionComment", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Diagnostic", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Denumire")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Denumire" }, "UQ__Diagnost__19701A4F23232DD2")
                        .IsUnique();

                    b.ToTable("Diagnostic", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Discussion", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("MessageContent");

                    b.Property<DateTime>("CommentDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CommentDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Discussion", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Entities.Valabilitate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("End")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SpecialistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Start")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkDayId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SpecialistId");

                    b.HasIndex("WorkDayId");

                    b.ToTable("DoctorSchedule");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Entities.WorkDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WorkDay");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("IdentityRole", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AdresaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NumberOfFailLoginAttempts")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumberCountryPrefix")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AdresaId");

                    b.ToTable("IdentityUser", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityUserToken", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsTokenRevoked")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshTokenValue")
                        .HasMaxLength(800)
                        .HasColumnType("nvarchar(800)");

                    b.Property<string>("TokenValue")
                        .IsRequired()
                        .HasMaxLength(800)
                        .HasColumnType("nvarchar(800)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("IdentityUserToken", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityUserTokenConfirmation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConfirmationToken")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<byte>("ConfirmationTypeId")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("IdentityUserTokenConfirmation", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IstoricDiagnosticUtilizator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DataDiagnosticare")
                        .HasColumnType("datetime");

                    b.Property<Guid>("DiagnosticId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DiagnosticId");

                    b.HasIndex("UserId");

                    b.ToTable("IstoricDiagnosticUtilizator", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AppointmentDate")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("SpecialistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppointmentStatus")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("AppointmentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SpecialistId");

                    b.HasIndex("UserId");

                    b.ToTable("Appointment", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Simptome", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Denumire")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Denumire" }, "UQ__Simptome__19701A4F05BDFAFB")
                        .IsUnique();

                    b.ToTable("Simptome", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Specialist", b =>
                {
                    b.Property<Guid>("SpecialistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Specialty")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SpecialistId");

                    b.ToTable("Specialist", (string)null);
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Pacient", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HasInsurance")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("SocialCategory")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Pacient", (string)null);
                });

            modelBuilder.Entity("SimptomeDiagnostic", b =>
                {
                    b.Property<Guid>("DiagnosticId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SimptomeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DiagnosticId", "SimptomeId");

                    b.HasIndex("SimptomeId");

                    b.ToTable("SimptomeDiagnostic", (string)null);
                });

            modelBuilder.Entity("UtilizatorSimptome", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SimptomeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "SimptomeId");

                    b.HasIndex("SimptomeId");

                    b.ToTable("UtilizatorSimptome", (string)null);
                });

            modelBuilder.Entity("IdentityUserIdentityRole", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MentalHealthApp.Entities.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MentalHealthApp.Entities.CameraConferintum", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Specialist", "Specialist")
                        .WithMany("CameraConferinta")
                        .HasForeignKey("SpecialistId")
                        .IsRequired()
                        .HasConstraintName("FK_SpecialistIdConf");

                    b.HasOne("MentalHealthApp.Entities.Pacient", "Pacient")
                        .WithMany("CameraConferinta")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UtilizatorIdConf");

                    b.Navigation("Specialist");

                    b.Navigation("Pacient");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.DiscussionComment", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Discussion", "Discussion")
                        .WithMany("DiscussionComments")
                        .HasForeignKey("DiscussionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MentalHealthApp.Entities.IdentityUser", "User")
                        .WithMany("DiscussionComments")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_ComentariiDiscutie_User_UserId");

                    b.Navigation("Discussion");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Discussion", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.IdentityUser", "User")
                        .WithMany("Discuties")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Discutie_User_UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Entities.Valabilitate", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Specialist", "Specialist")
                        .WithMany("DoctorSchedule")
                        .HasForeignKey("SpecialistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MentalHealthApp.Entities.Entities.WorkDay", "WorkDay")
                        .WithMany("DoctorSchedule")
                        .HasForeignKey("WorkDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specialist");

                    b.Navigation("WorkDay");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityUser", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Address", "Address")
                        .WithMany("IdentityUsers")
                        .HasForeignKey("AdresaId")
                        .IsRequired()
                        .HasConstraintName("FK_AdresaId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityUserToken", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.IdentityUser", "User")
                        .WithMany("IdentityUserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityUserTokenConfirmation", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.IdentityUser", "User")
                        .WithMany("IdentityUserTokenConfirmations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IstoricDiagnosticUtilizator", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Diagnostic", "Diagnostic")
                        .WithMany("IstoricDiagnosticUtilizators")
                        .HasForeignKey("DiagnosticId")
                        .IsRequired();

                    b.HasOne("MentalHealthApp.Entities.Utilizator", "Utilizator")
                        .WithMany("IstoricDiagnosticUtilizators")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Diagnostic");

                    b.Navigation("Pacient");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Appointment", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Specialist", "Specialist")
                        .WithMany("Appointments")
                        .HasForeignKey("SpecialistId")
                        .IsRequired()
                        .HasConstraintName("FK_SpecialistIdProg");

                    b.HasOne("MentalHealthApp.Entities.Pacient", "Pacient")
                        .WithMany("Appointments")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UtilizatorId");

                    b.Navigation("Specialist");

                    b.Navigation("Pacient");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Specialist", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.IdentityUser", "SpecialistNavigation")
                        .WithOne("Specialist")
                        .HasForeignKey("MentalHealthApp.Entities.Specialist", "SpecialistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_SpecialistId");

                    b.Navigation("SpecialistNavigation");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Pacient", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.IdentityUser", "User")
                        .WithOne("Pacient")
                        .HasForeignKey("MentalHealthApp.Entities.Pacient", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimptomeDiagnostic", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Diagnostic", null)
                        .WithMany()
                        .HasForeignKey("DiagnosticId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_DiagnosticSimptome_Diagnostic_DiagnosticId");

                    b.HasOne("MentalHealthApp.Entities.Simptome", null)
                        .WithMany()
                        .HasForeignKey("SimptomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_DiagnosticSimptome_Simptome_SimptomeId");
                });

            modelBuilder.Entity("UtilizatorSimptome", b =>
                {
                    b.HasOne("MentalHealthApp.Entities.Simptome", null)
                        .WithMany()
                        .HasForeignKey("SimptomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MentalHealthApp.Entities.Pacient", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UtilizatorSimptome_Utilizator_UserId");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Address", b =>
                {
                    b.Navigation("IdentityUsers");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Diagnostic", b =>
                {
                    b.Navigation("IstoricDiagnosticUtilizators");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Discussion", b =>
                {
                    b.Navigation("DiscussionComments");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Entities.WorkDay", b =>
                {
                    b.Navigation("DoctorSchedule");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.IdentityUser", b =>
                {
                    b.Navigation("DiscussionComments");

                    b.Navigation("Discuties");

                    b.Navigation("IdentityUserTokenConfirmations");

                    b.Navigation("IdentityUserTokens");

                    b.Navigation("Specialist")
                        .IsRequired();

                    b.Navigation("Pacient")
                        .IsRequired();
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Specialist", b =>
                {
                    b.Navigation("CameraConferinta");

                    b.Navigation("Appointments");

                    b.Navigation("DoctorSchedule");
                });

            modelBuilder.Entity("MentalHealthApp.Entities.Pacient", b =>
                {
                    b.Navigation("CameraConferinta");

                    b.Navigation("IstoricDiagnosticUtilizators");

                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
