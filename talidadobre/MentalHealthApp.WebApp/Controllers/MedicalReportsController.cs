using MentalHealthApp.BusinessLogic.Implementation.MedicalReports;
using MentalHealthApp.BusinessLogic.Implementation.MedicalReports.ViewModels;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthApp.WebApp.Controllers
{
    public class MedicalReportsController : BaseController
    {
        private readonly MedicalReportService _medicalReport;
        public MedicalReportsController(ControllerDependencies dependencies,
                                         MedicalReportService medicalReportsService) : base(dependencies)
        {
            _medicalReport = medicalReportsService;
        }

        [Authorize(Roles = "Specialist")]
        [HttpGet]
        public IActionResult CreateMedicalReport()
        {
            var model = new DoctorMedicalReportVM();
            return View("CreateMedicalReport", model);
        }

        [Authorize(Roles = "Specialist")]
        [HttpPost]
        public IActionResult CreateMedicalReport(DoctorMedicalReportVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            _medicalReport.CreateMedicalReport(model);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Pacient")]
        [HttpGet]
        public IActionResult ViewMedicalHistory()
        {
            var model = _medicalReport.ViewUserMedicalHistory();
            return View("UserHistory", model);
        }


        [Authorize(Roles = "Specialist")]
        [HttpGet]
        public IActionResult ViewPacientMedicalHistory(Guid id)
        {
            var model = _medicalReport.ViewPacientMedicalHistory(id);
            return View("UserHistory", model);
        }

        [HttpGet]
        public IActionResult ViewAReport(Guid id)
        {
            var model = _medicalReport.ViewMedicalHistoryById(id);
            ViewData["UserId"] = id;
            return View("MedicalReportView", model);
        }
    }
}
