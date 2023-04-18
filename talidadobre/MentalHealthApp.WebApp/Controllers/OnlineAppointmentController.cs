using AutoMapper;
using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.Admin;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthApp.WebApp.Controllers
{
    public class OnlineAppointmentController : BaseController
    {
        private readonly ForumService _forum;
        private readonly IMapper _mapper;
        public OnlineAppointmentController(ControllerDependencies dependencies,
                               ForumService forum,
                               IMapper mapper)
            : base(dependencies)
        {
            _mapper = mapper;
            _forum = forum;

        }

        [HttpGet]
        public IActionResult VideoDiscussion(int id)
        {
            var role = _forum.GetRole((Guid)CurrentUser.Id);
            ViewData["IsInitiator"] = role == "Specialist";
            ViewData["id"] = id;
            return View("VideoDiscussion");
        }

    }
}
