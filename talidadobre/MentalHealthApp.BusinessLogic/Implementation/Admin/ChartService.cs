using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Admin.Validations;
using MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels;
using MentalHealthApp.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin
{
    public class ChartService : BaseService
    {
        public ChartService(ServiceDependencies dependencies) : base(dependencies)
        {
        }

        public List<ChartDoctorModelVM> GetRatingInfoChartData()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Specialisti.Get()
                    .Include(p => p.DoctorReviews)
                    .Select(c => new ChartDoctorModelVM
                    {
                        SpecialistId = c.SpecialistId,
                        Name = uow.IdentityUsers.Get()
                            .Where(s => s.Id == c.SpecialistId)
                            .Select(s => s.FirstName + " " + s.LastName)
                            .First(),
                        Info = c.DoctorReviews
                            .Where(a => a.DoctorId == c.SpecialistId)
                            .Select(a => (int?)a.StarsNumber) 
                            .Average()
                            .HasValue ?
                                c.DoctorReviews
                                .Where(a => a.DoctorId == c.SpecialistId)
                                .Count() / 2 + c.DoctorReviews
                                    .Where(a => a.DoctorId == c.SpecialistId)
                                    .Average(a => a.StarsNumber) :  0
                                        })
                    .ToList();
            });
        }



        public List<ChartDoctorModelVM>?  GetAppointmentsInfoChartData()
        {
            return ExecuteInTransaction(uow =>
            {
                var result = new List<ChartDoctorModelVM>();
                var doctor =  uow.Specialisti.Get()
                                           .Include(p => p.Appointment)
                                           .ToList();

                if (doctor == null)
                {
                    return null;
                }

                var appointments = doctor
                .Select(c => new ChartHelperVM
                {
                    SpecialistId = c.SpecialistId,
                    Name = uow.IdentityUsers.Get()
                                            .Where(s => s.Id == c.SpecialistId)
                                            .Select(s => s.FirstName + " " + s.LastName)
                                            .First(),
                    Value1 = c.Appointment.Where(a => a.SpecialistId == c.SpecialistId && a.AppointmentStatus.Equals("Programare_Realizata")).Count(),
                    Value2 = c.Appointment.Where(a => a.SpecialistId == c.SpecialistId).Count(),
                }).ToList();
                foreach (var app in appointments)
                {
                    if(app.Value2 > 0)
                    {
                        var avg = app.Value1 / app.Value2 * 100;
                        var model = new ChartDoctorModelVM();
                        model.Info = avg;
                        model.Name = app.Name;
                        result.Add(model);
                    }
                   
                }
                return result;
            });

        }

        public List<DoctorChartInfoModel> GetSpecialistsList()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.IdentityUsers.Get()
                                       .Include(s => s.Roles)
                                       .Where(u => u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Specialist))
                                       .Select(s => new DoctorChartInfoModel
                                       {
                                           Name = s.LastName + " " + s.FirstName,
                                           Email = s.Email,
                                           Image = s.UserImage,
                                       }).ToList();
            });
        }
        public Dictionary<string, int> GetOnlineAppointmentsInfoChartData(string email)
        {
            return ExecuteInTransaction(uow =>
            {
                var doctor = uow.IdentityUsers.Get()
              .Include(p => p.Specialist)
              .ThenInclude(s => s.Appointment)
              .FirstOrDefault(c => c.Email.Equals(email));

                if (doctor == null)
                {
                    return null;
                }

                var appointmentsByStatus = doctor.Specialist.Appointment
                .Where(a => a.AppointmentType == "Online")
            .GroupBy(a => a.AppointmentStatus)
            .ToDictionary(g => g.Key, g => g.Count());

                return appointmentsByStatus;

            });

        }

        public Dictionary<string, int> GetF2FAppointmentsInfoChartData(string email)
        {
            return ExecuteInTransaction(uow =>
            {
                var doctor = uow.IdentityUsers.Get()
              .Include(p => p.Specialist)
              .ThenInclude(s => s.Appointment)
              .FirstOrDefault(c => c.Email.Equals(email));

                if (doctor == null)
                {
                    return null;
                }

                var appointmentsByStatus = doctor.Specialist.Appointment
                .Where(a => a.AppointmentType == "Fizic")
            .GroupBy(a => a.AppointmentStatus)
            .ToDictionary(g => g.Key, g => g.Count());

                return appointmentsByStatus;

            });

        }


    }
}
