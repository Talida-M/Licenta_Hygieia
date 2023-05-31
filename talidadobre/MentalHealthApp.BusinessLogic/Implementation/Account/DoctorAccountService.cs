using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Account.Validations;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.Common;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Common.Exceptions;
using MentalHealthApp.Common.Extensions;
using MentalHealthApp.DataAccess.Features;
using MentalHealthApp.Entities;
using MentalHealthApp.Entities.Entities;
using MentalHealthApp.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account
{
    public class DoctorAccountService : BaseService
    {
        private readonly IHashAlgo _hashAlgo;
        private readonly RegisterDoctorValidator _registerDoctorValidator;

        public DoctorAccountService(IHashAlgo hashAlgo, ServiceDependencies dependencies) :base(dependencies)
        {
            _hashAlgo = hashAlgo;
            _registerDoctorValidator = new RegisterDoctorValidator();
        }
      
            public List<EditDoctorDto> GetAllDoctors()
        {
            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Include(u => u.Address)
                                      .Include(u => u.Specialist)
                                      .Include(u => u.Roles).ToList();
                return query.Where(u => u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Specialist))
                                      .Select(u => new EditDoctorDto

                                       {
                                           LastName = u.LastName,
                                           FirstName = u.FirstName,
                                           Email = u.Email,
                                           PhoneNumber = u.PhoneNumber,
                                           Specialty = u.Specialist.Specialty,
                                           Description = u.Specialist.Description,
                                           AppointmentDate = u.Specialist.AppointmentDate,
                                           Price = u.Specialist.Price,
                                           Country = u.Address.Country,
                                           County = u.Address.County,
                                           City = u.Address.City,
                                           StreetNumber = u.Address.StreetNumber,
                                           ApartmentBuildingFloor = u.Address.ApartmentBuildingFloor,
                                           Sector = u.Address.Sector,
                                           ZipCode = u.Address.ZipCode

                                       })
                                      .ToList();

            });
        }

        public List<InactiveDoctor> GetInactiveDoctors()
        {
            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Include(u => u.Address)
                                      .Include(u => u.Specialist)
                                      //.ThenInclude(s => s.DoctorCVs)
                                      .Include(u => u.Roles)
                                      .ToList();
                var result = query.Where(u => u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Specialist) && u.IsActive == false && u.TwoFactorEnabled == false)
                                       .Select(u => new InactiveDoctor

                                       {
                                           SpecialistId = u.Id,
                                           LastName = u.LastName,
                                           FirstName = u.FirstName,
                                           Email = u.Email,
                                           PhoneNumber = u.PhoneNumber,
                                           Specialty = u.Specialist.Specialty,
                                           County = u.Address.County,
                                           City = u.Address.City,
                                           //CV = u.Specialist.DoctorCVs.DataFiles ?? new byte[0]
                                       })
                                      .ToList();
                foreach(var res in result)
                {
                    res.CV = uow.DoctorCVs.Get().Where(u => u.SpecialistId == res.SpecialistId).Select(u => u.DataFiles).First();
                    res.CVId = uow.DoctorCVs.Get().Where(u => u.SpecialistId == res.SpecialistId).Select(u => u.CVId).First();
                }
                return result;

            });
        }

        public bool ActivateUser(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Where(u => u.Id == id)
                                      .First();
                if(query != null)
                {
                    query.IsActive = true;
                    query.TwoFactorEnabled = true;
                    uow.IdentityUsers.Update(query);
                    uow.SaveChanges();
                    return true;
                }
                return false;

            });
        }

        public CVModel GetDoctorCV(int id)
        {
            return ExecuteInTransaction(uow =>
            {
                var query = uow.DoctorCVs.Get()
                                     .Where(u => u.CVId == id)
                                     .Select(u => new CVModel

                                     {
                                         SpecialistId = u.SpecialistId,
                                         Name = u.Name,
                                         FileType = u.FileType,
                                         DataFiles = u.DataFiles,
                                         CreatedOn = u.CreatedOn,                                       
                                     })
                                      .First();
                             
             
                return query;

            });
        }
        public List<EditDoctorDto> GetPageDoctors(int pages, int numberOnPage)
        {
            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Include(u => u.Address)
                                      .Include(u => u.Specialist)
                                      .Include(u => u.Roles).ToList();
                return query.Where(u => u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Specialist))
                .Skip(pages).Take(numberOnPage)
                .Select(u => new EditDoctorDto

                                      {
                                          LastName = u.LastName,
                                          FirstName = u.FirstName,
                                          Email = u.Email,
                                          PhoneNumber = u.PhoneNumber,
                                          Specialty = u.Specialist.Specialty,
                                          Description = u.Specialist.Description,
                                          AppointmentDate = u.Specialist.AppointmentDate,
                                          Country = u.Address.Country,
                                          County = u.Address.County,
                                          City = u.Address.City,
                                          Price = u.Specialist.Price,
                                          StreetNumber = u.Address.StreetNumber,
                                          ApartmentBuildingFloor = u.Address.ApartmentBuildingFloor,
                                          Sector = u.Address.Sector,
                                          ZipCode = u.Address.ZipCode

                                      })
                                      .ToList();

            });
        }

        public byte[] ConvertToBytes(IFormFile image)
        {
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            var imageBytes = reader.ReadBytes((int)image.Length);
            return imageBytes;
        }

        public double GetStarsAverageRating(Guid id)
        {
            var averageRating = UnitOfWork.DoctorReviews.Get().Where(sr => sr.DoctorId.Equals(id)).ToList();
            if (averageRating.Count > 0)
            {
                double averageRatingAverage = averageRating.Average(cd => cd.StarsNumber);
                return averageRatingAverage;
            }
            //
            return 0;
        }

        public List<string> GetDoctorsCities()
        {
            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Include(u => u.Address)
                                      .Include(u => u.Roles)
                                      .Where(u => u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Specialist) && u.IsActive == true)
                                      .Select(u => u.Address.City).Distinct().ToList();
                return query;
            });
        }
        public List<DoctorsCardsDto> GetDoctorsInfo()
        {
            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Include(u => u.Address)
                                      .Include(u => u.Specialist)
                                      .Include(u => u.Roles).ToList();
                return query.Where(u => u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Specialist) && u.IsActive == true)
                                      .Select(u => new DoctorsCardsDto

                                      {
                                          DoctorImage = u.UserImage,
                                          LastName = u.LastName,
                                          FirstName = u.FirstName,
                                          Email = u.Email,
                                          Specialitate = u.Specialist.Specialty,
                                          City = u.Address.City,
                                          Price = u.Specialist.Price,
                                          Rating = GetStarsAverageRating(u.Id)
                                      })
                                      .ToList();

            });
        }
      
        public void RegisterNewDoctor(RegisterDoctorModel model)
        {
            _registerDoctorValidator.Validate(model).ThenThrow(model);

            ExecuteInTransaction(uow =>
            {
                
                var user = uow.IdentityUsers.Get()
                                                    .Include(u => u.Roles)
                    .SingleOrDefault(u => u.Email.Equals(model.Email));
                if (user != null)
                {
                    string message = "Userul " + model.Email + " este deja inregistrat.";
                    throw new AlreadyExistsInDB(nameof(IdentityUser), message);
                }
                else
                {
                    var adresa = new Address();
                    adresa.Country = model.Country;
                    adresa.County = model.County;
                    adresa.City = model.City;
                    adresa.StreetNumber = model.StreetNumber;
                    adresa.ApartmentBuildingFloor = model.ApartmentBuildingFloor;
                    adresa.Sector = model.Sector;
                    adresa.ZipCode = model.ZipCode;
                    uow.Addresses.Insert(adresa);

                     

                    var registeredUser = new IdentityUser();
                    registeredUser.Id = Guid.NewGuid();
                    registeredUser.FirstName = model.FirstName;
                    registeredUser.LastName = model.LastName;
                    registeredUser.BirthDate = DateTime.UtcNow;
                    registeredUser.Email = model.Email;
                    registeredUser.EmailConfirmed = false;
                    registeredUser.PhoneNumberCountryPrefix = model.PhoneNumberCountryPrefix;
                    registeredUser.PhoneNumber = model.PhoneNumber;
                    registeredUser.PhoneNumberConfirmed = false;
                    registeredUser.PasswordHash = _hashAlgo.CalculateHashValueWithInput(model.PasswordHash);
                    registeredUser.TwoFactorEnabled = false;
                    registeredUser.CreatedAt = DateTime.UtcNow;
                    registeredUser.NumberOfFailLoginAttempts = 0;
                    registeredUser.IsActive = true;
                    registeredUser.IsDeleted = false;
                    registeredUser.Address = adresa;
                    registeredUser.UserImage = ConvertToBytes(model.UserImage);
                    registeredUser.Roles = new List<IdentityRole>
                {
                   uow.IdentityRoles.Get().Single(r => r.Id.Equals((int)RoleTypes.Specialist))
                }; ;
                    uow.IdentityUsers.Insert(registeredUser);
                    var specialist = new Specialist();
                    specialist.SpecialistId = registeredUser.Id;
                    specialist.Specialty = model.Specialty;
                    specialist.Description = model.Description;
                    specialist.AppointmentDate = model.AppointmentDate;
                    uow.Specialisti.Insert(specialist);

                    var fileName = Path.GetFileName(model.CV.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    var fileNamewithExtension = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    var cv = new DoctorCV();
                    cv.SpecialistId = specialist.SpecialistId;
                    cv.Name = Path.GetFileName(fileNamewithExtension);
                    cv.FileType = fileExtension;
                    using(var stream = new MemoryStream())
                    {
                        model.CV.CopyTo(stream);
                        cv.DataFiles = stream.ToArray();
                    }
                    cv.CreatedOn = DateTime.UtcNow;
                    uow.DoctorCVs.Insert(cv);
                    uow.SaveChanges();

                }
            });
        }


        public async Task<IdentityUser> EditDoctorInfo(EditDoctorDto editDoctor)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.IdentityUsers.Get()
                                          .Include(iu => iu.Address)
                                          .Include(iu => iu.Specialist)
                                          .SingleOrDefault(ui => ui.Email.Equals(editDoctor.Email));
                user.FirstName = editDoctor.FirstName;
                user.LastName = editDoctor.LastName;
                user.PhoneNumberCountryPrefix = editDoctor.PhoneNumberCountryPrefix;
                user.PhoneNumber = editDoctor.PhoneNumber;
                var adresa = user.Address;
                adresa.County = editDoctor.County;
                adresa.Country = editDoctor.Country;
                adresa.StreetNumber = editDoctor.StreetNumber;
                adresa.City = editDoctor.City;
                adresa.Sector = editDoctor.Sector;
                adresa.ApartmentBuildingFloor = editDoctor.ApartmentBuildingFloor;
                adresa.ZipCode = editDoctor.ZipCode;
                var specialist = user.Specialist;
                specialist.Description = editDoctor.Description;
                specialist.AppointmentDate = editDoctor.AppointmentDate;
                specialist.Price = editDoctor.Price;
                uow.IdentityUsers.Update(user);
                uow.SaveChanges();
                return user;

            });
        }
        public IdentityUser DeleteDoctor(string email)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.IdentityUsers.Get()
                            .Include(iu => iu.Address)
                            .Include(iu => iu.Roles)
                            .Include(iu => iu.Specialist)
                            .Where(iu => iu.Email.Equals(email))
                            .Single();
                var appointments = uow.Programari.Get().Where(p => p.SpecialistId.Equals(user.Id)).ToList();
                user.IsDeleted = true;
                foreach(var appointment in appointments)
                {
                    uow.Programari.Delete(appointment);
                }
                uow.Specialisti.Delete(user.Specialist);
                uow.IdentityUsers.Delete(user);
                uow.Addresses.Delete(user.Address);
                uow.SaveChanges();
                return user;
            });
        }
        public async Task<EditDoctorDto> GetDoctorByEmail (string email)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.IdentityUsers.Get()
                                               .Include(u => u.Address)
                                               .Include(u => u.Specialist)
                                               .Where(u => u.Email.Equals(email))
                                               .Select(u => new EditDoctorDto
                                               {
                                                   SpecialistId = u.Specialist.SpecialistId,
                                                   LastName = u.LastName,
                                                   FirstName = u.FirstName,
                                                   Email = u.Email,
                                                   PhoneNumber = u.PhoneNumber,
                                                   Specialty = u.Specialist.Specialty,
                                                   Description = u.Specialist.Description,
                                                   AppointmentDate = u.Specialist.AppointmentDate,
                                                   Country = u.Address.Country,
                                                   County = u.Address.County,
                                                   City = u.Address.City,
                                                   StreetNumber = u.Address.StreetNumber,
                                                   ApartmentBuildingFloor = u.Address.ApartmentBuildingFloor,
                                                   Sector = u.Address.Sector,
                                                   ZipCode = u.Address.ZipCode,
                                                   DoctorImage =  u.UserImage,
                                                   Price = u.Specialist.Price,

                                               }).SingleOrDefault();
                return user;
            });

        }
        public int GetDoctorProgramById(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.Specialisti.Get()
                                               .Where(u => u.SpecialistId.Equals(id))
                                               .Select(
                                                 u => u.AppointmentDate).SingleOrDefault();
                return user;
            });

        }
        public async Task<EditDoctorDto> GetDoctorById(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.IdentityUsers.Get()
                                               .Include(u => u.Address)
                                               .Include(u => u.Specialist)
                                               .Where(u => u.Id.Equals(id))
                                               .Select(u => new EditDoctorDto
                                               {
                                                   SpecialistId = u.Specialist.SpecialistId,
                                                   LastName = u.LastName,
                                                   FirstName = u.FirstName,
                                                   Email = u.Email,
                                                   PhoneNumber = u.PhoneNumber,
                                                   Specialty = u.Specialist.Specialty,
                                                   Description = u.Specialist.Description,
                                                   AppointmentDate = u.Specialist.AppointmentDate,
                                                   Country = u.Address.Country,
                                                   County = u.Address.County,
                                                   City = u.Address.City,
                                                   StreetNumber = u.Address.StreetNumber,
                                                   ApartmentBuildingFloor = u.Address.ApartmentBuildingFloor,
                                                   Sector = u.Address.Sector,
                                                   ZipCode = u.Address.ZipCode,
                                                   DoctorImage = u.UserImage, 
                                                   Price = u.Specialist.Price,

                                               }).SingleOrDefault();
                return user;
            });

        }
        public void AddWorkProgram(DoctorWorkProgramVM model)
        {
            ExecuteInTransaction(uow =>
            {
                var workDay = uow.DoctorSchedule.Get()
                                                .Include(x => x.WorkDay)
                                               .SingleOrDefault(v => v.WorkDay.Name.Equals(model.WorkDay) && v.SpecialistId.Equals(CurrentUser.Id));
                if (workDay != null)
                {
                    workDay.Start = model.Start;
                    workDay.End = model.End;
                    workDay.Notes = model.Notes;
                    uow.DoctorSchedule.Update(workDay);
                    uow.SaveChanges();
                    // string message = "Exsita deja o inregistrare pentru ziua respectiva" ;
                    //throw new AlreadyExistsInDB(nameof(Valabilitate), message);
                }
                else
                {
                    var specialist = uow.Specialisti.Get()
                                                     .Single(s => s.SpecialistId.Equals(CurrentUser.Id));
                    var newDailyProgram = new DoctorSchedule
                    {
                        Id = new Guid(),
                        SpecialistId = specialist.SpecialistId,
                        WorkDayId = uow.WorkDays.Get().Where(w => w.Name.Equals(model.WorkDay)).Select(w => w.Id).Single(),
                        Start = model.Start,
                        End = model.End,
                        Notes = model.Notes
                    };
                    UnitOfWork.DoctorSchedule.Insert(newDailyProgram);
                    uow.SaveChanges();
                }
            });
        }

        public List<DoctorWorkProgramVM> GetProgram()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.DoctorSchedule.Get()
                                        .Include(v => v.WorkDay)
                                        .Where(v => v.SpecialistId.Equals(CurrentUser.Id))
                                        .Select(v => new DoctorWorkProgramVM
                                        {
                                            Id = v.Id,
                                            SpecialistId = v.SpecialistId,
                                            WorkDayId = v.WorkDayId,
                                            WorkDay = v.WorkDay.Name,
                                            Start = v.Start,
                                            End = v.End,
                                            Notes = v.Notes
                                        }).ToList();
            });
        }
        public List<DoctorWorkProgramVM> GetDoctorProgram(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.DoctorSchedule.Get()
                                        .Include(v => v.WorkDay)
                                        .Where(v => v.SpecialistId.Equals(id))
                                        .Select(v => new DoctorWorkProgramVM
                                        {
                                            Id = v.Id,
                                            SpecialistId = v.SpecialistId,
                                            WorkDayId = v.WorkDayId,
                                            WorkDay = v.WorkDay.Name,
                                            Start = v.Start,
                                            End = v.End,
                                            Notes = v.Notes
                                        }).ToList();
            });
        }
        public List<WorkDaysVM> GetWorkDays()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.WorkDays.Get()
                                        .Select(v => new WorkDaysVM
                                        {
                                            Id = v.Id,
                                            Name = v.Name,
                                        }).ToList();
            });
        }

    }
}
