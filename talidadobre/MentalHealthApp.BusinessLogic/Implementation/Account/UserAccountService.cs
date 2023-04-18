using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Account.Validations;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.Common;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Common.Exceptions;
using MentalHealthApp.Common.Extensions;
using MentalHealthApp.DataAccess;
using MentalHealthApp.DataAccess.Features;
using MentalHealthApp.Entities;
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
    public class UserAccountService :BaseService
    {
        private readonly IHashAlgo _hashAlgo;
        private readonly RegisterUserValidator _registerUserValidator;

        public UserAccountService(IHashAlgo hashAlgo, ServiceDependencies dependencies) :base(dependencies)
        {
            _hashAlgo = hashAlgo;
            _registerUserValidator = new RegisterUserValidator();   
        }

        public List<EditUserDto> GetAllUsers()
        {

            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Include(u => u.Address)
                                      .Include(u => u.Pacient)
                                      .Include(u => u.Roles).ToList();
                return query.Where(u => u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Pacient))
                            .Select(u => new EditUserDto
                                     {
                                         LastName = u.LastName,
                                         FirstName = u.FirstName,
                                         BirthDate = u.BirthDate,
                                         Email = u.Email,
                                         PhoneNumber = u.PhoneNumber,
                                         Username = u.Pacient.Username,
                                         SocialCategory = u.Pacient.SocialCategory,
                                         HasInsurance = u.Pacient.HasInsurance,
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

        public List<EditUserDto> GetPageWithUsers(int pages, int numberOnPage)
        {

            return ExecuteInTransaction(uow =>
            {
                var query = uow.IdentityUsers.Get()
                                      .Include(u => u.Address)
                                      .Include(u => u.Pacient)
                                      .Include(u => u.Roles).ToList();
                return query.Where(u => u.Pacient != null  && u.Roles.Select(r => r.Id).Contains((int)RoleTypes.Pacient))
                .Skip(pages).Take(numberOnPage)
                .Select(u => new EditUserDto
                            {
                                LastName = u.LastName,
                                FirstName = u.FirstName,
                                BirthDate = u.BirthDate,
                                Email = u.Email,
                                PhoneNumber = u.PhoneNumber,
                                Username = u.Pacient.Username,
                                SocialCategory = u.Pacient.SocialCategory,
                                HasInsurance = u.Pacient.HasInsurance,
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
        public CurrentUserDto? Login(string email, string password)
        {
            var userPass = UnitOfWork.IdentityUsers.Get().Where(u => u.Email.Equals(email) && u.IsDeleted.Equals(false)).Select(u => u.PasswordHash).SingleOrDefault();
            if (userPass is null)
            {
                return null;
            }
            string initialSalt = userPass.Split('.')[1];
            bool isVerified = _hashAlgo.IsPasswordVerified(userPass, initialSalt, password);
            var user = UnitOfWork.IdentityUsers.Get()
                                                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Email == email);
          
            if (isVerified == true)
            {
                return new CurrentUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    isAuthenticated = true,
                    Roles = user.Roles.Select(ur => ur.Name).ToList()
                };
            }
            return new CurrentUserDto { isAuthenticated = false };
        }
        public byte[]? GetUserImage()
        {
            return ExecuteInTransaction(uow =>
            {
               return uow.IdentityUsers.Get()
                            .Where(u => u.Id == CurrentUser.Id)
                            .Select(u => u.UserImage)
                           .SingleOrDefault();
            });
        }
        public byte[] ConvertToBytes(IFormFile image)
        {
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            var imageBytes = reader.ReadBytes((int)image.Length);
            return imageBytes;
        }
        public void RegisterNewUser(RegisterModel model)
        {
            _registerUserValidator.Validate(model).ThenThrow(model);
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
                registeredUser.BirthDate = model.BirthDate;
                registeredUser.Email = model.Email;
                registeredUser.EmailConfirmed = false;
                registeredUser.PhoneNumberCountryPrefix = model.PhoneNumberCountryPrefix;
                registeredUser.PhoneNumber = model.PhoneNumber;
                registeredUser.PhoneNumberConfirmed = false;
                registeredUser.PasswordHash = _hashAlgo.CalculateHashValueWithInput(model.PasswordHash);
                registeredUser.TwoFactorEnabled = false;
                registeredUser.CreatedAt = DateTime.UtcNow;
                registeredUser.NumberOfFailLoginAttempts = 0;
                registeredUser.IsActive = false;
                registeredUser.IsDeleted = false;
                registeredUser.Address = adresa;
                    registeredUser.UserImage = ConvertToBytes(model.UserImage);
                    registeredUser.Roles = new List<IdentityRole>
                {
                   uow.IdentityRoles.Get().Single(r => r.Id.Equals((int)RoleTypes.Pacient))
                };
                    uow.IdentityUsers.Insert(registeredUser);

                    var utilizator = new Pacient();
                    utilizator.UserId = registeredUser.Id;
                    utilizator.Username = model.Username;
                    utilizator.SocialCategory = model.SocialCategory;
                    utilizator.HasInsurance = model.HasInsurance;
                uow.Utilizatori.Insert(utilizator);
                uow.SaveChanges();

            }
        });
        }

      
        public List<ListItemModel<string, Guid>> GetUsers()
        {
            return UnitOfWork.IdentityUsers.Get()
                .Select(u => new ListItemModel<string, Guid>
                {
                    Text = $"{u.FirstName} {u.LastName}",
                    Value = u.Id
                })
                .ToList();
        }
       
        public IdentityUser DeleteUser (string email)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.IdentityUsers.Get()
                            .Include(iu => iu.Address)
                            .Include(iu => iu.Roles)
                            .Include(iu => iu.Pacient)
                            .Where(iu => iu.Email.Equals(email))
                            .FirstOrDefault();
                user.IsDeleted = true;
                uow.IdentityUsers.Update(user);
                uow.Utilizatori.Delete(user.Pacient);
                uow.SaveChanges();
                return user;
            });
        }
        public async Task<IdentityUser> ChangePassword(IdentityUser user, string password)
        {
            string updatedPassword = _hashAlgo.CalculateHashValueWithInput(password);
            if (updatedPassword != null)
            {
                user.PasswordHash = updatedPassword;
                UnitOfWork.SaveChanges();
                return user;
            }
            return null;
        }
    }
}
