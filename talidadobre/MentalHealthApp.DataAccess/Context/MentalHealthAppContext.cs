using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MentalHealthApp.Entities;
using MentalHealthApp.Entities.Entities;

namespace MentalHealthApp.DataAccess.Context
{
    public partial class MentalHealthAppContext : DbContext
    {
        public MentalHealthAppContext()
        {
        }

        public MentalHealthAppContext(DbContextOptions<MentalHealthAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<CameraConferintum> CameraConferinta { get; set; } = null!;
        public virtual DbSet<DiscussionComment> DiscussionComments { get; set; } = null!;
        public virtual DbSet<Diagnostic> Diagnostics { get; set; } = null!;
        public virtual DbSet<Discussion> Discuties { get; set; } = null!;
        public virtual DbSet<IdentityRole> IdentityRoles { get; set; } = null!;
        public virtual DbSet<IdentityUser> IdentityUsers { get; set; } = null!;
        public virtual DbSet<IdentityUserToken> IdentityUserTokens { get; set; } = null!;
        public virtual DbSet<IdentityUserTokenConfirmation> IdentityUserTokenConfirmations { get; set; } = null!;
        public virtual DbSet<IstoricDiagnosticUtilizator> IstoricDiagnosticUtilizators { get; set; } = null!;
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Simptome> Simptomes { get; set; } = null!;
        public virtual DbSet<Specialist> Specialists { get; set; } = null!;
        public virtual DbSet<Pacient> Pacients { get; set; } = null!;
        public virtual DbSet<DoctorSchedule> DoctorSchedule { get; set; } = null!;
        public virtual DbSet<TopReads> TopReads { get; set; } = null!;
        public virtual DbSet<ConditionsPost> ConditionsPosts { get; set; } = null!;
        public virtual DbSet<MedicalReport> MedicalReports { get; set; } = null!;
        public virtual DbSet<DoctorReviews> DoctorReviews { get; set; } = null!;
        public virtual DbSet<UserJournal> UserJournals { get; set; } = null!;
        public virtual DbSet<DoctorCV> DoctorCVs { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-H551OQK\\SQLEXPRESS;Initial Catalog=MentalHealthProject;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.ApartmentBuildingFloor).HasMaxLength(50);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.County).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Sector).HasMaxLength(50);

                entity.Property(e => e.StreetNumber).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(50);
            });

            modelBuilder.Entity<MedicalReport>(entity =>
            {
                entity.ToTable("MedicalReport");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ReportDate)
                .HasColumnType("datetime")
                .HasColumnName("raportDate");

                entity.Property(e => e.ReportDescription)
                    .HasMaxLength(800)
                    .HasColumnName("reportDescription");

                entity.Property(e => e.MedicalHistory)
                .HasColumnName("medicalHistory")
                .HasMaxLength(800);

                entity.Property(e => e.Condition)
                    .HasColumnName("condition")
                    .HasMaxLength(50);

                entity.Property(e => e.Strategy)
                    .HasColumnName("strategy")
                    .HasMaxLength(800);

                entity.Property(e => e.Prescription)
                .HasColumnName("prescription")
                .HasMaxLength(800);

                entity.Property(e => e.DoctorNotes)
                    .HasColumnName("doctorNotes")
                    .HasMaxLength(200);

                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.MedicalReports)
                    .HasForeignKey(d => d.PacientId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Specialist)
                    .WithMany(p => p.MedicalReports)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicalReport_SpecialistId");
            });

            modelBuilder.Entity<UserJournal>(entity =>
            {
                entity.ToTable("UserJournal");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PageDate)
                .HasColumnType("datetime")
                .HasColumnName("PageDate");

                entity.Property(e => e.Content)
                    .HasColumnName("Content");

                entity.Property(e => e.IsPublic)
                .HasColumnName("IsPublic")
                .HasColumnType("bit");


                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.UserJournals)
                    .HasForeignKey(d => d.PacientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserJournal_PacientId");
            });

            modelBuilder.Entity<DoctorCV>(entity =>
            {
                entity.ToTable("DoctorCV");

                entity.Property(e => e.CVId);

                entity.Property(e => e.Name)
                .HasColumnName("Name");

                entity.Property(e => e.FileType)
                  .HasColumnName("FileType");

                entity.Property(e => e.DataFiles)
                    .HasColumnType("varbinary(MAX)")
                    .HasColumnName("DataFiles");

                entity.Property(e => e.CreatedOn)
                .HasColumnName("CreatedOn")
                .HasColumnType("datetime");

                modelBuilder.Entity<DoctorCV>()
                    .HasOne(d => d.Specialist)
                    .WithOne(p => p.DoctorCVs)
                    .HasForeignKey<DoctorCV>(d => d.SpecialistId)
                    .HasPrincipalKey<Specialist>(p => p.SpecialistId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PaymentDetails>(entity =>
            {
                entity.ToTable("PaymentDetails");

                entity.Property(e => e.Id);

                entity.Property(e => e.PaymentDate)
                .HasColumnName("PaymentDate")
                .HasColumnType("datetime"); ;

                entity.Property(e => e.Amount)
                  .HasColumnName("Amount")
                  .HasColumnType("int");

                entity.Property(e => e.Currency)
                    .HasColumnType("nvarchar(3)")
                    .HasColumnName("Currency");

                entity.Property(e => e.PaymentStatus)
                .HasColumnName("PaymentStatus")
                .HasColumnType("nvarchar(20)");

                modelBuilder.Entity<PaymentDetails>()
                    .HasOne(d => d.Appointment)
                    .WithOne(p => p.PaymentDetails)
                    .HasForeignKey<PaymentDetails>(d => d.AppointmentId)
                    .HasPrincipalKey<Appointment>(p => p.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CameraConferintum>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.OraInceput).HasMaxLength(20);

                entity.Property(e => e.OraSfarsit).HasMaxLength(20);

                entity.HasOne(d => d.Specialist)
                    .WithMany(p => p.CameraConferinta)
                    .HasForeignKey(d => d.SpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecialistIdConf");

                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.CameraConferinta)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UtilizatorIdConf");
            });

            modelBuilder.Entity<DiscussionComment>(entity =>
            {
                entity.ToTable("DiscussionComment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.MessageContent).HasColumnName("MessageContent");

                entity.Property(e => e.CommentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CommentDate");

                entity.HasOne(d => d.Discussion)
                    .WithMany(p => p.DiscussionComments)
                    .HasForeignKey(d => d.DiscussionId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DiscussionComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComentariiDiscutie_User_UserId");
            });
            modelBuilder.Entity<DoctorReviews>(entity =>
            {
                entity.ToTable("DoctorReviews");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Review).HasColumnName("Review").HasMaxLength(500);

                entity.Property(e => e.CommentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CommentDate");

                entity.Property(e => e.StarsNumber)
                    .HasColumnType("int")
                    .HasColumnName("StarsNumber");

                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.DoctorReviews)
                    .HasForeignKey(d => d.PacientId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_DoctorReviews_Pacient"); ;

                entity.HasOne(d => d.Specialist)
                    .WithMany(p => p.DoctorReviews)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_DoctorReviews_Specialist");
            });




            modelBuilder.Entity<Diagnostic>(entity =>
            {
                entity.ToTable("Diagnostic");

                entity.HasIndex(e => e.Denumire, "UQ__Diagnost__19701A4F23232DD2")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Denumire).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.HasMany(d => d.Simptomes)
                    .WithMany(p => p.Diagnostics)
                    .UsingEntity<Dictionary<string, object>>(
                        "SimptomeDiagnostic",
                        l => l.HasOne<Simptome>().WithMany().HasForeignKey("SimptomeId").HasConstraintName("FK_DiagnosticSimptome_Simptome_SimptomeId"),
                        r => r.HasOne<Diagnostic>().WithMany().HasForeignKey("DiagnosticId").HasConstraintName("FK_DiagnosticSimptome_Diagnostic_DiagnosticId"),
                        j =>
                        {
                            j.HasKey("DiagnosticId", "SimptomeId");

                            j.ToTable("SimptomeDiagnostic");
                        });
            });

            modelBuilder.Entity<Discussion>(entity =>
            {
                entity.ToTable("Discussion");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.MessageContent).HasColumnName("MessageContent");

                entity.Property(e => e.CommentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CommentDate");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("Title");

                entity.Property(e => e.Topic)
                    .HasColumnName("Topic");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Discuties)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Discutie_User_UserId");
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("IdentityRole");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable("IdentityUser");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PasswordHash).HasMaxLength(500);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.PhoneNumberCountryPrefix).HasMaxLength(6);
               
                entity.HasOne(d => d.Address)
                    .WithMany(p => p.IdentityUsers)
                    .HasForeignKey(d => d.AdresaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdresaId");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "IdentityUserIdentityRole",
                        l => l.HasOne<IdentityRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<IdentityUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("IdentityUserIdentityRole");
                        });
               entity.Property(e => e.UserImage).HasColumnType("varbinary(MAX)");
            });

            modelBuilder.Entity<IdentityUserToken>(entity =>
            {
                entity.ToTable("IdentityUserToken");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RefreshTokenValue).HasMaxLength(800);

                entity.Property(e => e.TokenValue).HasMaxLength(800);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.IdentityUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<IdentityUserTokenConfirmation>(entity =>
            {
                entity.ToTable("IdentityUserTokenConfirmation");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ConfirmationToken).HasMaxLength(500);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.IdentityUserTokenConfirmations)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<IstoricDiagnosticUtilizator>(entity =>
            {
                entity.ToTable("IstoricDiagnosticUtilizator");

                entity.Property(e => e.DataDiagnosticare).HasColumnType("datetime");

                entity.HasOne(d => d.Diagnostic)
                    .WithMany(p => p.IstoricDiagnosticUtilizators)
                    .HasForeignKey(d => d.DiagnosticId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.IstoricDiagnosticUtilizators)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentDate).HasMaxLength(50);


                entity.Property(e => e.AppointmentStatus).HasMaxLength(20);

                entity.Property(e => e.AppointmentType).HasMaxLength(50);

                entity.HasOne(d => d.Specialist)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.SpecialistId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SpecialistIdProg");

                entity.HasOne(d => d.Pacient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UtilizatorId");
            });
            

            modelBuilder.Entity<Simptome>(entity =>
            {
                entity.ToTable("Simptome");

                entity.HasIndex(e => e.Denumire, "UQ__Simptome__19701A4F05BDFAFB")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Denumire).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<Specialist>(entity =>
            {
                entity.ToTable("Specialist");

                entity.Property(e => e.SpecialistId).ValueGeneratedNever();

                entity.Property(e => e.Specialty).HasMaxLength(100);

                entity.HasOne(d => d.SpecialistNavigation)
                    .WithOne(p => p.Specialist)
                    .HasForeignKey<Specialist>(d => d.SpecialistId)
                    .HasConstraintName("FK_SpecialistId");
            });

            modelBuilder.Entity<Pacient>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Pacient");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.HasInsurance).HasMaxLength(5);

                entity.Property(e => e.SocialCategory).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Pacient)
                    .HasForeignKey<Pacient>(d => d.UserId)
                    .HasConstraintName("FK_UserId");

                entity.HasMany(d => d.Simptomes)
                    .WithMany(p => p.Pacients)
                    .UsingEntity<Dictionary<string, object>>(
                        "UtilizatorSimptome",
                        l => l.HasOne<Simptome>().WithMany().HasForeignKey("SimptomeId"),
                        r => r.HasOne<Pacient>().WithMany().HasForeignKey("UserId").HasConstraintName("FK_UtilizatorSimptome_Utilizator_UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "SimptomeId");

                            j.ToTable("UtilizatorSimptome");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
