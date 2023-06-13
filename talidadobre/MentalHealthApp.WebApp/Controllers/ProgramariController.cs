using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using MentalHealthApp.Entities.Enums;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using System.Configuration;

namespace MentalHealthApp.WebApp.Controllers
{
    public class ProgramariController : BaseController
    {
        private readonly ProgramareService _programareService;
        private readonly DoctorAccountService _doctorAccountService;
        private readonly IConfiguration _configuration;

        public ProgramariController(ControllerDependencies dependencies,
                                 ProgramareService programareService,
                                 DoctorAccountService doctorAccountService) : base(dependencies)
        { 
            _programareService = programareService;
            _doctorAccountService = doctorAccountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Appointments()
        {

            var model = _programareService.GetAllAppointments();
            return View(model);
        }

        [Authorize(Roles = "Pacient, Specialist" )]
        [HttpPost]
        public IActionResult CreateAppointment(AppointmentModel model)
        {

            var check =  _programareService.CheckDate(model.AppointmentDate, model.AppointmentHour, model.SpecialistId);
            if(check == false)
            {
                ViewBag.ErrorMessage = "Appointment time does not check doctor work program";
                return RedirectToAction("PageNotFound", "Home");
                //return StatusCode(StatusCodes.Status409Conflict, "Appointment time does not check doctor work program");
            }
            _programareService.AddDoctorAppointment(model);
            return RedirectToAction("Appointments", "Profile", new { id = model.SpecialistId });
        }


    }
}
