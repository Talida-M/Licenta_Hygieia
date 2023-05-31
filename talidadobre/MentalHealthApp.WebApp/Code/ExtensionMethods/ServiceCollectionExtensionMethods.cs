using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.WebApp.Code.Base;
using System;
using System.Linq;
using MentalHealthApp.DataAccess.Features;
using Microsoft.EntityFrameworkCore;
using MentalHealthApp.Entities;
using MentalHealthApp.DataAccess;
using System.Security.Claims;
using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Admin;
using MentalHealthApp.BusinessLogic.Implementation.MedicalReports;
using MentalHealthApp.BusinessLogic.Implementation.Reviews;
using MentalHealthApp.BusinessLogic.Implementation.UserJournals;
using MentalHealthApp.BusinessLogic.Implementation.PayPal;

namespace MentalHealthApp.WebApp.Code.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<ControllerDependencies>();
            return services;
        }

        public static IServiceCollection AddMentalHealthAppBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ServiceDependencies>();
            services.AddScoped<UserAccountService>();
            services.AddScoped<DoctorAccountService>();
            services.AddScoped<UserProfileService>();
            services.AddScoped<ProgramareService>();
            services.AddScoped<IHashAlgo, HashAlg>();
            services.AddScoped<ForumService>();
            services.AddScoped<HomePageService>();
            services.AddScoped<MedicalReportService>();
            services.AddScoped<DoctorReviewService>();
            services.AddScoped<ChartService>();
            services.AddScoped<UserJournalService>();
            services.AddScoped<PaymentService>();
            return services;
        }
        public static IServiceCollection AddMentalHealthAppCurrentUser(this IServiceCollection services)
        {
            services.AddScoped(s =>
            {
                var accessor = s.GetService<IHttpContextAccessor>();
                var httpContext = accessor.HttpContext;
                var claims = httpContext.User.Claims;


                var userIdClaim = claims?.SingleOrDefault(c => c.Type == "Id")?.Value;
                var isParsingSuccessful = Guid.TryParse(userIdClaim, out Guid id);
                var userEmail = claims?.SingleOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
                var userName = claims?.SingleOrDefault(e => e.Type == ClaimTypes.Name)?.Value;

                var uow = s.GetService<UnitOfWork>()!;
                var userImage = uow.IdentityUsers
                    .Get()
                    .SingleOrDefault(ui => ui.Email == userEmail)?.UserImage;
                return new CurrentUserDto
                {
                    isAuthenticated = httpContext.User.Identity.IsAuthenticated,
                    Email = userEmail,
                    FirstName = userName,
                    Id =  Guid.TryParse(userIdClaim, out var userIdClaimAsGuid) ?
                            userIdClaimAsGuid : null,
                    UserImage = userImage


                };
            });

            return services;
        }
    }
}
