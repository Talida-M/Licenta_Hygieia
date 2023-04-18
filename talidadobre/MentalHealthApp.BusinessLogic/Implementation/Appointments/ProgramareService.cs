using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.Common.Exceptions;
using MentalHealthApp.Entities.Enums;
using MentalHealthApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using System.Data;


namespace MentalHealthApp.BusinessLogic.Implementation
{
    public class ProgramareService : BaseService
    {
        private readonly DoctorAccountService _doctorAccountService;
        public ProgramareService(ServiceDependencies dependencies,
                                DoctorAccountService doctorAccountService) : base(dependencies)
        {
            _doctorAccountService = doctorAccountService;
        }


        public string GetDoctorNameById(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var name = uow.IdentityUsers.Get().Where(n => n.Id.Equals(id)).Select(n => $"{n.LastName} {n.FirstName}").Single();
                return name;
            });
        }
        public void AddDoctorAppointment(AppointmentModel model)
        {
            ExecuteInTransaction(uow =>
            {

                var programare = uow.Programari.Get()
                                               .SingleOrDefault(s => s.AppointmentDate.Equals(model.AppointmentDate));
                if(programare != null)
                {
                    string message = "Exista deja o programare pentru  " + model.AppointmentDate ;
                    throw new AlreadyExistsInDB(nameof(Appointment), message);
                }
                else
                {
                    var specialist = uow.Specialisti.Get().Single(c => c.SpecialistId.Equals(model.SpecialistId));
                    var utilizatorId = uow.IdentityUsers
                                        .Get()
                                        .Include(iu => iu.Pacient)
                                        .Single(iu => iu.Id.Equals(CurrentUser.Id.Value))
                                        .Pacient.UserId;
                    var user = uow.Utilizatori.Get().Single(c => c.UserId.Equals(utilizatorId));
                    var newAppointment = new Appointment
                    {
                       // OraProgramare = model.OraProgramare,
                        AppointmentDate = model.AppointmentDate + " " + model.AppointmentHour,
                        AppointmentType = model.AppointmentType,
                        AppointmentStatus = AppointmentStatus.In_Asteptare.ToString(),
                        Specialist = specialist,
                        Pacient = user
                    };
                    uow.Programari.Insert(newAppointment);
                    uow.SaveChanges();
                }

            });
        }
        public IEnumerable<AppointmentsAdminModel> GetAllAppointmentsAdmin()
        {
            return ExecuteInTransaction(uow =>
            {
              
                var appointments = uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Include(p => p.Specialist)
                                     .Select(u => new AppointmentsAdminModel
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,

                                         //  OraProgramare = u.OraProgramare,
                                         SpecialistId = u.SpecialistId,
                                         DoctorName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         UserId = u.UserId,
                                         UserName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         AppointmentType = u.AppointmentType,
                                         AppointmentStatus = u.AppointmentStatus
                                     });
                var result = appointments.OrderBy(u => Convert.ToDateTime(u.AppointmentDate).Date)
                                     .OrderBy(u => Convert.ToDateTime(u.AppointmentDate).Hour);
                return result;


            });
        }

        public IEnumerable<AppointmentsAdminModel> GetOnePageAppointmentsAdmin(int pages, int pageSize)
        {
            return ExecuteInTransaction(uow =>
            {

                var appointments = uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Include(p => p.Specialist)
                                      .Skip(pages).Take(pageSize)
                                     .Select(u => new AppointmentsAdminModel
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,

                                         //  OraProgramare = u.OraProgramare,
                                         SpecialistId = u.SpecialistId,
                                         DoctorName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         UserId = u.UserId,
                                         UserName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         AppointmentType = u.AppointmentType,
                                         AppointmentStatus = u.AppointmentStatus
                                     }).ToList();
                
                var result = appointments.OrderBy(u => Convert.ToDateTime(u.AppointmentDate).Date)
                                     .OrderBy(u => Convert.ToDateTime(u.AppointmentDate).Hour);
                return result;


            });
        }
        public List<AppointmentsAdminModel> SearchAppointmentsByDoctorName(string doctorName)
        {
            return ExecuteInTransaction(uow =>
            {
                var appointments =  uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Include(p => p.Specialist)
                                     .Select(u => new AppointmentsAdminModel
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         SpecialistId = u.SpecialistId,
                                         DoctorName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         UserId = u.UserId,
                                         UserName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         AppointmentType = u.AppointmentType,
                                         AppointmentStatus = u.AppointmentStatus
                                     })
                                    .ToList();
                var result = appointments.Where(u => u.DoctorName.Contains(doctorName)).ToList().OrderBy(u => Convert.ToDateTime(u.AppointmentDate).Date)
                                     .OrderBy(u => Convert.ToDateTime(u.AppointmentDate).Hour).ToList(); 
                return result;

            });
        }

        public bool CheckDate(string appointmentDate, string appointmentTime, Guid doctorId)
        {
            var time = _doctorAccountService.GetDoctorProgramById(doctorId);
            var countAppointments = GetAcceptedAppointments(doctorId)
                                .Where(u => Convert.ToDateTime(u.AppointmentDate.Split(" ")[0])
                                .Equals(Convert.ToDateTime(appointmentDate))
                                       && (Convert.ToDateTime(appointmentTime).Minute - Convert.ToDateTime(u.AppointmentDate).Minute < time));
                                //.Count();
            if (countAppointments.Count() > 0)
            {
                return false;
            }

            var lista = _doctorAccountService.GetDoctorProgram(doctorId);
            if(lista == null)
            {
                return true;
            }
            foreach (var item in lista)
            {
                if (Convert.ToDateTime(item.Start).Hour > Convert.ToDateTime(appointmentTime).Hour && item.WorkDay == Convert.ToDateTime(appointmentDate).DayOfWeek.ToString())
                {
                    return false;

                  
                }
                if((Convert.ToDateTime(item.Start).Hour == Convert.ToDateTime(appointmentTime).Hour 
                && Convert.ToDateTime(item.Start).Minute > Convert.ToDateTime(appointmentDate).Minute) 
                && item.WorkDay == Convert.ToDateTime(appointmentDate).DayOfWeek.ToString())
                {
                    return false;
                }
                   
                if ((Convert.ToDateTime(item.End).Hour < Convert.ToDateTime(appointmentTime).Hour 
                || (Convert.ToDateTime(item.End).Hour == Convert.ToDateTime(appointmentTime).Hour) 
                && Convert.ToDateTime(item.End).Minute < Convert.ToDateTime(appointmentTime).Minute) 
                && item.WorkDay == Convert.ToDateTime(appointmentDate).DayOfWeek.ToString())
                {
                    return false;
                } 

            }
            
            return true;
            }
        public IEnumerable<DoctorNameModel> GetDoctorsName()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                      .Include(p => p.Specialist)
                                     .Select(u => new DoctorNameModel
                                     {
                                         SpecialistId = u.SpecialistId,
                                         DoctorName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),

                                     }).Distinct();
                          
                

            });
        }

        public List<ProgramareDto> GetAllAppointments()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                      .Include(p => p.Specialist)
                                      .Where(p => p.UserId.Equals(CurrentUser.Id.Value))
                                     .Select(u => new ProgramareDto
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         Doctor = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         AppointmentType = u.AppointmentType,
                                         AppointmentStatus = u.AppointmentStatus,
                                         
                                     })
                                    .ToList();
                                       

            });
        }

        public Tuple<string, string> GetSpecialistInfo(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var result = uow.IdentityUsers.Get()
                                      .Include(p => p.Specialist)
                                      .FirstOrDefault(p => p.Id.Equals(id));

                if(result != null)
                {
                    return Tuple.Create(result.Specialist.AppointmentDate.ToString(), result.Specialist.Price.ToString());
                }
                return Tuple.Create("-", "-");
            });
        }
        public List<ProgramareProfilMedicDto> GetAllAwaitingAppointments()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Where(p => p.SpecialistId.Equals(CurrentUser.Id.Value) && p.AppointmentStatus==AppointmentStatus.In_Asteptare.ToString())
                                     .Select(u => new ProgramareProfilMedicDto
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         Pacient = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         AppointmentStatus = u.AppointmentStatus
                                     })
                                    .ToList();


            });
        }

        public List<AppointmentsDoctorProfileVM> GetAcceptedAppointments(Guid doctorId)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Where(p => !(p.UserId.Equals(CurrentUser.Id.Value)) && p.SpecialistId.Equals(doctorId) && p.AppointmentStatus == AppointmentStatus.Programare_Acceptata.ToString())
                                     .Select(u => new AppointmentsDoctorProfileVM
                                     {
                                         AppointmentDate = u.AppointmentDate,
                                     })
                                    .ToList();


            });
        }
        public List<AcceptedAppointmentsDto> GetAllOnlineAppointments()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Where(p => p.SpecialistId.Equals(CurrentUser.Id.Value) && p.AppointmentStatus != AppointmentStatus.In_Asteptare.ToString() && p.AppointmentType == "Online")
                                     .Select(u => new AcceptedAppointmentsDto
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         Pacient = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         Email = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.Email).Single(),
                                         BirthDate = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.BirthDate).Single(),
                                         PhoneNumber = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.PhoneNumber).Single(),
                                         SocialCategory = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.SocialCategory).Single(),
                                         HasInsurance = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.HasInsurance).Single(),
                                         AppointmentStatus = u.AppointmentStatus, 
                                         UserImage = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.UserImage).Single(),
                                     })
                                    .ToList();


            });
        }
        public List<AcceptedAppointmentsDto> GetAllPhysicalAppointments()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Where(p => p.SpecialistId.Equals(CurrentUser.Id.Value) && p.AppointmentStatus != AppointmentStatus.In_Asteptare.ToString() && p.AppointmentType == "Fizic")
                                     .Select(u => new AcceptedAppointmentsDto
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         Pacient = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         Email = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.Email).Single(),
                                         BirthDate = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.BirthDate).Single(),
                                         PhoneNumber = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.PhoneNumber).Single(),
                                         SocialCategory = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.SocialCategory).Single(),
                                         HasInsurance = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.HasInsurance).Single(),
                                         AppointmentStatus = u.AppointmentStatus
                                     })
                                    .ToList();


            });
        }
        public List<AcceptedAppointmentsDto> GetTodayAppointments()
        {
            return ExecuteInTransaction(uow =>
            {
                var appointments = uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Where(p => p.SpecialistId.Equals(CurrentUser.Id.Value) && p.AppointmentType == "Online" && p.AppointmentStatus == AppointmentStatus.Programare_Acceptata.ToString())
                                      .ToList()
                                      .Select(u => new AcceptedAppointmentsDto
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         Pacient = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         Email = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.Email).Single(),
                                         BirthDate = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.BirthDate).Single(),
                                         PhoneNumber = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.PhoneNumber).Single(),
                                         SocialCategory = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.SocialCategory).Single(),
                                         HasInsurance = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.HasInsurance).Single(),
                                         UserImage = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.UserImage).Single()
                                      })
                                     .OrderBy(u => Convert.ToDateTime(u.AppointmentDate))
                                    .ToList();

                var result = appointments.Where(u => Convert.ToDateTime(u.AppointmentDate).Date == DateTime.Now.Date).ToList();
                return result;


            });
        }


        public List<PacientVM> GetPacients()
        {
            return ExecuteInTransaction(uow =>
            {
                var pacienti = uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Where(p => p.SpecialistId.Equals(CurrentUser.Id.Value) && ( p.AppointmentStatus == AppointmentStatus.Programare_Acceptata.ToString() || p.AppointmentStatus == AppointmentStatus.Programare_Realizata.ToString()))
                                      .ToList()
                                      .Select(u => new PacientVM
                                      {
                                          UserId = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.Id).Single(),
                                          FullName = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                          Email = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.Email).Single(),
                                          BirthDate = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.BirthDate).Single(),
                                          PhoneNumber = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.PhoneNumber).Single(),
                                          SocialCategory = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.SocialCategory).Single(),
                                          HasInsurance = uow.Utilizatori.Get().Where(n => n.UserId.Equals(u.UserId)).Select(n => n.HasInsurance).Single(),
                                          UserImage = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.UserId)).Select(n => n.UserImage).Single()
                                      })
                                     .OrderBy(u => u.FullName)
                                     .ToList();
                var result = pacienti
           .GroupBy(p => p.UserId)
           .Select(g => g.First())
           .ToList();
                return result;


            });
        }

        public List<AcceptedAppointmentsDto> GetUserTodayAppointments()
        {
            return ExecuteInTransaction(uow =>
            {
                var appointments = uow.Programari.Get()
                                      .Include(p => p.Specialist)
                                      .Where(p => p.UserId.Equals(CurrentUser.Id.Value) && p.AppointmentType == "Online" && p.AppointmentStatus == AppointmentStatus.Programare_Acceptata.ToString())
                                      .ToList()
                                      .Select(u => new AcceptedAppointmentsDto
                                      {
                                          Id = u.Id,
                                          AppointmentDate = u.AppointmentDate,
                                          Specialist = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                          Email = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => n.Email).Single(),
                                          PhoneNumber = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => n.PhoneNumber).Single(),
                                          UserImage = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => n.UserImage).Single()
                                      })
                                     .OrderBy(u => Convert.ToDateTime(u.AppointmentDate))
                                    .ToList();

                var result = appointments.Where(u => Convert.ToDateTime(u.AppointmentDate).Date == DateTime.Now.Date).ToList();
                return result;


            });
        }

        public int AwaitingAppoinments()
        {
            return ExecuteInTransaction(uow =>
            {
              return  uow.Programari.Get()
                                      .Include(p => p.Pacient)
                                      .Where(p => p.SpecialistId.Equals(CurrentUser.Id.Value) && p.AppointmentStatus == AppointmentStatus.In_Asteptare.ToString())
                                      .Count();

            });
        }
        public  string EditStatus(int AppoitmentId, string newStatus)
        {
            return ExecuteInTransaction(uow =>
            {
                var status = uow.Programari.Get()
                                            .Where(p => p.Id.Equals(AppoitmentId))
                                            .SingleOrDefault();
                status.AppointmentStatus = newStatus;
                uow.Programari.Update(status);
                uow.SaveChanges();
                return newStatus.ToString();
            });

        }
        public string EditTime(int AppoitmentId, string newStatus)
        {
            return ExecuteInTransaction(uow =>
            {
                var status = uow.Programari.Get()
                                            .Where(p => p.Id.Equals(AppoitmentId))
                                            .SingleOrDefault();
                status.AppointmentDate = newStatus;
                uow.Programari.Update(status);
                uow.SaveChanges();
                return newStatus;
            });

        }
        public async Task<Appointment> DeleteAppointment(int id)
        {

            return ExecuteInTransaction(uow =>
            {

                var app = uow.Programari.Get()
                                                .Where(cd => cd.Id.Equals(id))
                                                .Single();
                uow.Programari.Delete(app);
                uow.SaveChanges();
                return app;

            });
        }
    }
}
