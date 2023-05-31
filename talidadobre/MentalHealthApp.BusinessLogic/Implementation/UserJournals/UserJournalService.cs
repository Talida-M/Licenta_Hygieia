using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Reviews.ViewModel;
using MentalHealthApp.BusinessLogic.Implementation.UserJournals.ViewModel;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Entities;
using MentalHealthApp.Entities.Entities;
using MentalHealthApp.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.UserJournals
{
    public class UserJournalService : BaseService
    {
        public UserJournalService(ServiceDependencies dependencies) : base(dependencies) { }

        public void NewJournalPage(string content, bool confidentialitate)
        {
            ExecuteInTransaction(uow =>
            {
                var page = new UserJournal();
                page.Id = Guid.NewGuid();
                page.PacientId = (Guid)CurrentUser.Id;
                page.Content = content;
                page.PageDate = DateTime.UtcNow;
                page.IsPublic = confidentialitate;
                uow.UserJournals.Insert(page);
                uow.SaveChanges();
            });

        }

        public UserJournal DeletePageContent(Guid id)
        {

            return ExecuteInTransaction(uow =>
            {

                var page = uow.UserJournals.Get()
                                                .Where(cd => cd.Id.Equals(id))
                                                .Single();
                uow.UserJournals.Delete(page);
                uow.SaveChanges();
                return page;

            });
        }

        public List<JournalPageVM> ViewUserJournal()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.UserJournals.Get()
                                             .Where(cd => cd.PacientId.Equals(CurrentUser.Id))
                                             .Select(cd => new JournalPageVM
                                             {
                                                 PageDate = cd.PageDate,
                                                 Content = cd.Content,
                                                 IsPublic = cd.IsPublic,
                                                 Id = cd.Id
                                             })
                                             .OrderBy(u => u.PageDate)
                                             .ToList();
            });
        }

        public List<Guid> ViewDoctorPacientsId()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                             .Where(cd => cd.SpecialistId.Equals(CurrentUser.Id) && (cd.AppointmentStatus == AppointmentStatus.Programare_Realizata.ToString() || cd.AppointmentStatus == AppointmentStatus.Programare_Acceptata.ToString()))
                                             .Select(cd => cd.UserId)
                                             .Distinct()
                                             .ToList();
            });
        }

        public List<JournalPageVM> ViewPacientJournalById(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.UserJournals.Get()
                                             .Where(cd => cd.PacientId.Equals(id) && cd.IsPublic == true)
                                             .Select(cd => new JournalPageVM
                                             {
                                                 PageDate = cd.PageDate,
                                                 Content = cd.Content,
                                                 IsPublic = cd.IsPublic,

                                             })
                                             .OrderBy(u => u.PageDate)
                                             .ToList();
            });
        }


        public bool ChangeDiagnosticPublicField(bool isPublic, Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var result =  uow.UserJournals.Get()
                                             .Where(cd => cd.Id.Equals(id)).First();
                if (result == null)
                {
                    return false;
                }
                result.IsPublic = isPublic;
                uow.UserJournals.Update(result);
                uow.SaveChanges();
                return true;
            });
        }

        public bool EditPageContent(Guid id, string content)
        {
            return ExecuteInTransaction(uow =>
            {
                var result = uow.UserJournals.Get()
                                             .Where(cd => cd.Id.Equals(id)).First();
                if (result == null)
                {
                    return false;
                }
                result.Content = content;
                uow.UserJournals.Update(result);
                uow.SaveChanges();
                return true;
            });
        }
    }
}
