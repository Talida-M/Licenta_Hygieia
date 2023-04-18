using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account
{
    public class UserProfileService :BaseService
    {
        private readonly UserAccountService _user;
      public UserProfileService(ServiceDependencies dependencies,
                                UserAccountService userAccount ) 
                                : base(dependencies)
        {
            _user = userAccount;
        }

        public  IdentityUser EditUserInfo(EditUserDto editUser)
        {
                return ExecuteInTransaction(uow =>
                {
                    var user = uow.IdentityUsers.Get()
                                           .Include(iu => iu.Address)
                                           .Include(iu => iu.Pacient)
                                           .SingleOrDefault(ui => ui.Email.Equals(editUser.Email));
                    user.FirstName = editUser.FirstName;
                    user.LastName = editUser.LastName;
                    user.BirthDate = editUser.BirthDate;
                   // user.PhoneNumberCountryPrefix = editUser.PhoneNumberCountryPrefix;
                    var adresa = user.Address;
                    adresa.County = editUser.County;
                    adresa.Country = editUser.Country;
                    adresa.StreetNumber = editUser.StreetNumber;
                    adresa.City = editUser.City;
                    adresa.ApartmentBuildingFloor = editUser.ApartmentBuildingFloor;
                    adresa.ZipCode = editUser.ZipCode;
                    adresa.Sector = editUser.Sector;
                    var utilizator = user.Pacient;
                    utilizator.Username = editUser.Username;
                    utilizator.HasInsurance = editUser.HasInsurance;
                    utilizator.SocialCategory = editUser.SocialCategory;

                    uow.IdentityUsers.Update(user);
                    uow.SaveChanges();
                    return user;

                });
            }

        public IFormFile ChangeProfilePhoto(IFormFile userImage)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.IdentityUsers.Get()
                                       .SingleOrDefault(ui => ui.Id.Equals(CurrentUser.Id));
                user.UserImage = _user.ConvertToBytes(userImage);
                uow.IdentityUsers.Update(user);
                uow.SaveChanges();
                return userImage;

            });
        }

        public async Task<UserVM> GetUserByEmail(string email)
        {
            return ExecuteInTransaction(uow =>
            {
                var user = uow.IdentityUsers.Get()
                                     .Include(u => u.Address)
                                     .Include(u => u.Pacient)
                                     .Where(u => u.Email == email)
                                     .Select(u => new UserVM
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
                                         ZipCode = u.Address.ZipCode,
                                         UserImage = u.UserImage
                                         

                                         
                                     })
                                    .SingleOrDefault();
                return user;
            });
            
        }
    }
}
