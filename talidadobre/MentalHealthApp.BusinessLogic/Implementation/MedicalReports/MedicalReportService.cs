using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.MedicalReports.ViewModels;
using MentalHealthApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.MedicalReports
{
    public class MedicalReportService : BaseService
    {
        private readonly DoctorAccountService _doctorAccountService;
        public MedicalReportService(ServiceDependencies dependencies,
                                DoctorAccountService doctorAccountService) : base(dependencies)
        {
            _doctorAccountService = doctorAccountService;
        }

        public void CreateMedicalReport(DoctorMedicalReportVM model)
        {
            ExecuteInTransaction(uow =>
            {
                var newReport = new MedicalReport();
                newReport.Id = Guid.NewGuid();
                newReport.PacientId = uow.IdentityUsers.Get()
                                                        .Where(u => u.Email.Equals(model.PacientEmail))
                                                        .Select(u => u.Id)
                                                        .Single();
                newReport.DoctorId = (Guid)CurrentUser.Id;
                newReport.ReportDate = model.ReportDate;
                newReport.ReportDescription = model.ReportDescription;
                newReport.MedicalHistory = model.MedicalHistory;
                newReport.Condition = model.Condition;
                newReport.Strategy = model.Strategy;
                newReport.Prescription = model.Prescription;
                newReport.DoctorNotes = model.DoctorNotes;
                uow.MedicalReports.Insert(newReport);
                uow.SaveChanges();
            });
        }

        public List<DoctorMedicalReportVM> ViewDoctorReports()
        {
            return ExecuteInTransaction(uow =>
            {
                var reports = uow.MedicalReports.Get()
                                                 .Include(md => md.Pacient)
                                                 .Where(md => md.DoctorId.Equals(CurrentUser.Id))
                                                 .Select(u => new DoctorMedicalReportVM
                                                 {
                                                     Id = u.Id,
                                                     Doctor = CurrentUser.FirstName.ToString(),
                                                     Pacient = uow.IdentityUsers.Get().Where(iu => iu.Id.Equals(u.Pacient.UserId)).Select(iu => $"{iu.FirstName} {iu.LastName}").Single(),
                                                     ReportDate = u.ReportDate,
                                                     MedicalHistory = u.MedicalHistory,
                                                     Condition = u.Condition,
                                                     Prescription = u.Prescription,
                                                     ReportDescription = u.ReportDescription,
                                                     Strategy = u.Strategy,
                                                     DoctorNotes = u.DoctorNotes

                                                 }).ToList();
                var finalResult = reports.OrderByDescending(u => u.ReportDate).ToList();
                return finalResult;
            });
        }

        public List<PacientMedicalReportVM> ViewUserMedicalHistory()
        {
            return ExecuteInTransaction(uow =>
            {
                var reports = uow.MedicalReports.Get()
                                                 .Include(md => md.Specialist)
                                                 .Where(md => md.PacientId.Equals(CurrentUser.Id))
                                                 .Select(u => new PacientMedicalReportVM
                                                 {
                                                     Id = u.Id,
                                                     Pacient = CurrentUser.FirstName.ToString(),
                                                     Doctor = uow.IdentityUsers.Get().Where(iu => iu.Id.Equals(u.Specialist.SpecialistId)).Select(iu => $"{iu.FirstName} {iu.LastName}").Single(),
                                                     ReportDate = u.ReportDate,
                                                     MedicalHistory = u.MedicalHistory,
                                                     Condition = u.Condition,
                                                     Prescription = u.Prescription
                                                 }).ToList();
                var finalResult = reports.OrderByDescending(u => u.ReportDate).ToList();
                return finalResult;
            });
        }
        public PacientMedicalReportVM ViewMedicalHistoryById(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var report = uow.MedicalReports.Get()
                                                 .Include(md => md.Specialist)
                                                 .Where(md => md.Id.Equals(id))
                                                 .Select(u => new PacientMedicalReportVM
                                                 {
                                                     Id = u.Id,
                                                     Pacient = CurrentUser.FirstName.ToString(),
                                                     Doctor = uow.IdentityUsers.Get().Where(iu => iu.Id.Equals(u.Specialist.SpecialistId)).Select(iu => $"{iu.FirstName} {iu.LastName}").Single(),
                                                     ReportDate = u.ReportDate,
                                                     ReportDescription = u.ReportDescription,
                                                     MedicalHistory = u.MedicalHistory,
                                                     Condition = u.Condition,
                                                     Prescription = u.Prescription
                                                 }).Single();
                return report;
            });
        }
    }
}
   
