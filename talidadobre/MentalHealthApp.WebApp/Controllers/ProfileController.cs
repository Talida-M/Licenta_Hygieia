using AutoMapper;
using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Reviews;
using MentalHealthApp.BusinessLogic.Implementation.Reviews.ViewModel;
using MentalHealthApp.BusinessLogic.Implementation.UserJournals;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Entities.Enums;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthApp.WebApp.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly UserProfileService _userProfileService;
        private readonly DoctorAccountService _doctorAccountService;
        private readonly ProgramareService _programareService;
        private readonly DoctorReviewService _doctorReviewService;
        private readonly UserJournalService _userJournalService;

        private readonly IMapper _mapper;
        public ProfileController(ControllerDependencies dependencies,
                                UserProfileService userProfileService,
                                DoctorAccountService doctorAccountService,
                                IMapper mapper,
                                ProgramareService programareService,
                                DoctorReviewService doctorReviewService,
                                UserJournalService userJournalService

                                )
                                : base(dependencies)
        {
            _userProfileService = userProfileService;
            _doctorAccountService = doctorAccountService;
            _mapper = mapper;
            _programareService = programareService;
            _doctorReviewService = doctorReviewService;
            _userJournalService = userJournalService;
        }

        [HttpGet]
        public async  Task<IActionResult> UserProfile()
        {
            var profile =  await _userProfileService.GetUserByEmail(CurrentUser.Email);
            return View(profile);
    
        }

        [HttpPost]
        public IActionResult EditUserProfile(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userDto = _mapper.Map<EditUserDto>(model);
            _userProfileService.EditUserInfo(userDto);
            return RedirectToAction("UserProfile", model);
        }
        [HttpGet]
        public async Task<IActionResult> EditUserProfile(string email)
        {
            var model = await _userProfileService.GetUserByEmail(email);
            return View("EditUserProfile",model);
        }

        [HttpPatch]
        public  IActionResult ChangeProfilePhoto(IFormFile userImage)
        {
            var model =  _userProfileService.ChangeProfilePhoto(userImage);
            return Ok(userImage);
        }
        [HttpGet]
        //[Authorize(Roles = "Specialist")]
        public async Task<IActionResult> DoctorProfile()
        {
            var profile = await _doctorAccountService.GetDoctorByEmail(CurrentUser.Email);
            return View("DoctorAccountProfile", profile);

        }
        [HttpGet]
        public  IActionResult DoctorsProfile()
        {
            var profile = _doctorAccountService.GetDoctorsInfo();
            return View("DoctorsProfile", profile);

        }
        [HttpGet]
        public IActionResult UserProfileMap()
        {
            return View("UserProfileMap");

        }

        [HttpGet]
        public IActionResult UserJournal()
        {
            var appointments = _userJournalService.ViewUserJournal();
            return View("UserJournal", appointments);

        }
        [HttpGet]
        public async Task<IActionResult> DoctorInfo(string email)
        {
            var profile = await  _doctorAccountService.GetDoctorByEmail(email);
            // var model = _mapper.Map<DoctorVM>(profile);
            var rating = _doctorReviewService.GetStarsAverageRating(profile.SpecialistId);
            var reviewsModel = _doctorReviewService.ViewDoctorReviews(profile.SpecialistId);
            ViewData["Rating"] = rating;
            ViewData["NewRating"] = new NewReviewVM();
            if(reviewsModel != null)
            {
                ViewData["reviewsModel"] = reviewsModel;

            }
            return View("DoctorInfo", profile);

        }
        [HttpGet]
        public async Task<IActionResult> DoctorInfoById(Guid id)
        {
            var profile = await _doctorAccountService.GetDoctorById(id);
            // var model = _mapper.Map<DoctorVM>(profile);
            return View("DoctorInfo", profile);

        }

        [HttpGet]
        public IActionResult Appointments(Guid id)
        {
            var listaProgramari = _programareService.GetAllAppointments();
            var listaData = _programareService.GetAcceptedAppointments(id);
            var lista = _doctorAccountService.GetDoctorProgram(id);
            var doctorInfo = _programareService.GetSpecialistInfo(id);
            var model = new AppointmentModel()
            {

                SpecialistId = id,
                AppointmentTime = doctorInfo.Item1,
                Price = doctorInfo.Item2,
                AppointmentsVMs = listaProgramari.Select(app => new AppointmentsVM
                {
                    Id = app.Id,
                    AppointmentDate = app.AppointmentDate,
                    Doctor = app.Doctor,
                    AppointmentType = app.AppointmentType,
                    AppointmentStatus = app.AppointmentStatus.Split("_")[0] + " " + app.AppointmentStatus.Split("_")[1]

                }).ToList(),
                AcceptedAppointmentsData = listaData.Select(ld => new AppointmentsDoctorProfileVM
                {
                    AppointmentDate = ld.AppointmentDate
                }).ToList(),
                DoctorWorkProgramVMs  = lista.Select(dwp => new DoctorWorkProgramVM
                {
                    Id = dwp.Id,
                    SpecialistId = dwp.SpecialistId,
                    WorkDayId = dwp.WorkDayId,
                    WorkDay = dwp.WorkDay,
                    Start = dwp.Start,
                    End = dwp.End,
                    Notes = dwp.Notes
                }).ToList()
            };

            return View("Appointments", model);

        }

        [HttpGet]
        public IActionResult UserTodayAppointments()
        {
            var appointments = _programareService.GetUserTodayAppointments();
            var model = new AcceptedAppointmentsVM()
            {


                AcceptedAppointmentsVMs = appointments.Select(app => new AcceptedAppointmentsVM
                {
                    Id = app.Id,
                    AppointmentDate = app.AppointmentDate,
                    Specialist = app.Specialist,
                    Email = app.Email,
                    PhoneNumber = app.PhoneNumber,
                    UserImage = app.UserImage

                }).ToList()
            };
            return View("UserTodayAppointments", model);
        }

        //////////////////////
        [HttpPost]
        public  IActionResult CreateReview(string review, int stars, Guid doctorId)
        {

            _doctorReviewService.NewReview(review, stars, doctorId);
            return View();
        }
    }
}
