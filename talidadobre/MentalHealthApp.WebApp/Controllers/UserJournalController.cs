using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.WebApp.Code.Base;
using MentalHealthApp.BusinessLogic.Implementation.UserJournals;
using System.Data;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using MentalHealthApp.BusinessLogic.Implementation.UserJournals.ViewModel;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace MentalHealthApp.WebApp.Controllers
{
    public class UserJournalController : BaseController
    {
        private readonly UserJournalService _userJournalService;
        public UserJournalController(ControllerDependencies dependencies
                                     , UserJournalService userJournalService)
                                       : base(dependencies)
        {
            _userJournalService = userJournalService;
        }

        [System.Web.Mvc.Authorize(Roles = "Pacient")]
        [System.Web.Mvc.HttpGet]
        public IActionResult GetJournalPagesForCurrentUSer()
        {
            var journal = _userJournalService.ViewUserJournal();
            return View("UserJournal", journal);
        }

      

        [Authorize(Roles = "Pacient")]
        [HttpPost]
        public IActionResult CreateJournalPage(string content, bool confidentialitate)
        {

           _userJournalService.NewJournalPage(content, confidentialitate);
            return Ok();
        }


        [Authorize(Roles = "Pacient")]
        [System.Web.Mvc.HttpPatch]
        public IActionResult ChangePublicField(bool isPublic, Guid id)
        {

            _userJournalService.ChangeDiagnosticPublicField(isPublic, id);
            return Ok();
        }

    }
}