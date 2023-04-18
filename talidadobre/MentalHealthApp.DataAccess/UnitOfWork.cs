using MentalHealthApp.Common;
using MentalHealthApp.DataAccess.Context;
using MentalHealthApp.Entities;
using MentalHealthApp.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.DataAccess
{
    public class UnitOfWork
    {
        private readonly MentalHealthAppContext Context;

        public UnitOfWork(MentalHealthAppContext context)
        {
            this.Context = context;
        }
        private IRepository<IdentityUser> identityUsers;
        public IRepository<IdentityUser> IdentityUsers => identityUsers ?? (identityUsers = new BaseRepository<IdentityUser>(Context));

        private IRepository<IdentityRole> identityRoles;
        public IRepository<IdentityRole> IdentityRoles => identityRoles ?? (identityRoles = new BaseRepository<IdentityRole>(Context));


        private IRepository<Specialist> specialisti;
        public IRepository<Specialist> Specialisti => specialisti ?? (specialisti = new BaseRepository<Specialist>(Context));

        private IRepository<Pacient> utilizatori;
        public IRepository<Pacient> Utilizatori => utilizatori ?? (utilizatori = new BaseRepository<Pacient>(Context));

        private IRepository<Appointment> programari;
        public IRepository<Appointment> Programari => programari ?? (programari = new BaseRepository<Appointment>(Context));

        private IRepository<IstoricDiagnosticUtilizator> istoricDiagnosticUtilizatori;
        public IRepository<IstoricDiagnosticUtilizator> IstoricDiagnosticUtilizator => istoricDiagnosticUtilizatori ?? (istoricDiagnosticUtilizatori = new BaseRepository<IstoricDiagnosticUtilizator>(Context));

        private IRepository<Discussion> discutii;
        public IRepository<Discussion> Discutii => discutii ?? (discutii = new BaseRepository<Discussion>(Context));

        private IRepository<DiscussionComment> comentariiDiscutii;
        public IRepository<DiscussionComment> ComentariiDiscutii => comentariiDiscutii ?? (comentariiDiscutii = new BaseRepository<DiscussionComment>(Context));

        private IRepository<CameraConferintum> cameraConferinte;
        public IRepository<CameraConferintum> CameraConferinte => cameraConferinte ?? (cameraConferinte = new BaseRepository<CameraConferintum>(Context));

        private IRepository<Address> adrese;
        public IRepository<Address> Addresses => adrese ?? (adrese = new BaseRepository<Address>(Context));

        private IRepository<Diagnostic> diagnostics;
        public IRepository<Diagnostic> Diagnostics => diagnostics ?? (diagnostics = new BaseRepository<Diagnostic>(Context));

        private IRepository<Simptome> simptom;
        public IRepository<Simptome> Simptom => simptom ?? (simptom = new BaseRepository<Simptome>(Context));

        private IRepository<DoctorSchedule> valabilitati;
        public IRepository<DoctorSchedule> DoctorSchedule => valabilitati ?? (valabilitati = new BaseRepository<DoctorSchedule>(Context));
        
        private IRepository<WorkDay> workDays;
        public IRepository<WorkDay> WorkDays => workDays ?? (workDays = new BaseRepository<WorkDay>(Context));

        private IRepository<ConditionsPost> conditionPosts;
        public IRepository<ConditionsPost> ConditionsPosts => conditionPosts ?? (conditionPosts = new BaseRepository<ConditionsPost>(Context));

        private IRepository<TopReads> topReads;
        public IRepository<TopReads> TopReads => topReads ?? (topReads = new BaseRepository<TopReads>(Context));

        private IRepository<MedicalReport> medicalReports;
        public IRepository<MedicalReport> MedicalReports => medicalReports ?? (medicalReports = new BaseRepository<MedicalReport>(Context));

        private IRepository<DoctorReviews> doctorReviews;
        public IRepository<DoctorReviews> DoctorReviews => doctorReviews ?? (doctorReviews = new BaseRepository<DoctorReviews>(Context));

        private IRepository<UserJournal> userJournals;
        public IRepository<UserJournal> UserJournals => userJournals ?? (userJournals = new BaseRepository<UserJournal>(Context));

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
