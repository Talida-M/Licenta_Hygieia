using AutoMapper;
using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.PayPal.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.PayPal;
using MentalHealthApp.BusinessLogic.Implementation.Reviews;
using MentalHealthApp.BusinessLogic.Implementation.Reviews.ViewModel;
using MentalHealthApp.BusinessLogic.Implementation.UserJournals;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Entities.Enums;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using System.Configuration;
using MentalHealthApp.BusinessLogic.Base;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MentalHealthApp.WebApp.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly UserProfileService _userProfileService;
        private readonly DoctorAccountService _doctorAccountService;
        private readonly ProgramareService _programareService;
        private readonly DoctorReviewService _doctorReviewService;
        private readonly UserJournalService _userJournalService;
        private readonly IConfiguration _configuration;
        private readonly PaymentService _paymentService;

        private readonly IMapper _mapper;
        public ProfileController(ControllerDependencies dependencies,
                                UserProfileService userProfileService,
                                DoctorAccountService doctorAccountService,
                                IMapper mapper,
                                ProgramareService programareService,
                                DoctorReviewService doctorReviewService,
                                UserJournalService userJournalService,
                                IConfiguration configuration,
                               PaymentService paymentService
                                )
                                : base(dependencies)
        {
            _userProfileService = userProfileService;
            _doctorAccountService = doctorAccountService;
            _mapper = mapper;
            _programareService = programareService;
            _doctorReviewService = doctorReviewService;
            _userJournalService = userJournalService;
            _configuration = configuration;
            _paymentService = paymentService;

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
            var cities = _doctorAccountService.GetDoctorsCities();
            ViewBag.CitiesList = cities;
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
            if(profile == null)
            {
                ViewBag.ErrorMessage = "We're sorry, the page you were looking for isn't found here. The link you followed may either be broken or the doctor infos no longer exists.";
                return RedirectToAction("PageNotFound", "Home");
            }
            return View("DoctorInfo", profile);

        }

        [HttpGet]
        public IActionResult GetFilteredAppointments(string doctorId, string selectedDate)
        {
            var lista = _programareService.GetFilteredAppointments(Guid.Parse(doctorId), selectedDate);
            return Ok(lista);
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
        public IActionResult UserTodayAppointments(bool val = false)
        {
            var appointments = _programareService.GetUserTodayAppointments();
            ViewBag.TransactionIsMade = val;
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
        public  IActionResult CreateReview(string review, int stars, string email)
        {

            _doctorReviewService.NewReview(review, stars, email);
            return View();
        }


        //////////
        public async Task<IActionResult> InitiatePayment(int id)//int pret
        {
            // Create a new APIContext instance using your PayPal credentials
                    var apiContext = new APIContext(new OAuthTokenCredential(
            //_configuration["PayPalSettings:SandboxClientId"],
            //_configuration["PayPalSettings:SandboxClientSecret"]
                        _configuration["PayPalSettings:ClientId"],
                        _configuration["PayPalSettings:ClientSecret"]
            ).GetAccessToken());
            var appointmentResult = _paymentService.GetAppointmentDetail(id);
            // Create a new payment object
            var conversion = (double)await _paymentService.ConvertCurrencyAsync(appointmentResult.Pret, "RON", "USD");
            if ((int)conversion == 0)
            {
                conversion = 1.49;
            }
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
        {
            new Transaction
            {
                amount = new Amount
                {
                    total =  conversion.ToString(),//appointmentResult.Pret != 0 ? appointmentResult.Pret.ToString()  : "5", //pret.ToString(),
                    currency = "USD"
                },
                description = "Plata realizata de catre " + appointmentResult.Pacient + " pentru consultatia din " + appointmentResult.AppointmentDate + " la medicul " + appointmentResult.Doctor ,
            }
        },
                redirect_urls = new RedirectUrls
                {
                    return_url = Url.Action("ExecutePayment", "ProfileController", new { id }),
                    cancel_url = "https://example.com/cancel"
                }
            };

            // Create the payment
            var createdPayment = payment.Create(apiContext);

            // Redirect the user to the PayPal payment approval URL
            return Redirect(createdPayment.links.FirstOrDefault(l => l.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase)).href);
        }


        public IActionResult ExecutePayment(int id, string paymentId, string token, string PayerID)
        {
            // Create a new APIContext instance using your PayPal credentials
                    var apiContext = new APIContext(new OAuthTokenCredential(
           //_configuration["PayPalSettings:SandboxClientId"],
           //_configuration["PayPalSettings:SandboxClientSecret"]
                           _configuration["PayPalSettings:SandboxClientId"],
                           _configuration["PayPalSettings:SandboxClientSecret"]
           ).GetAccessToken());

            // Retrieve the payment by paymentId
            var payment = Payment.Get(apiContext, paymentId);
            var execution = new PaymentExecution { payer_id = PayerID };
            //var executedPayment = payment.Execute(apiContext, execution);
            // Execute the payment
            var executedPayment = payment.Execute(apiContext, new PaymentExecution { payer_id = PayerID });
           // var id = _paymentService.GetAppointmentByPacient().Id;
            // Handle the payment execution response
            if (executedPayment.state == "approved")
            {
                // Payment is successful
                // Perform necessary actions (e.g., update database, send confirmation email, etc.)

                var paymentModel = new PaymentModel
                {
                    Amount = int.Parse(payment.transactions[0].amount.total),
                    AppointmentId = id, // Replace with your actual appointment ID
                    Currency = payment.transactions[0].amount.currency,
                    PaymentStatus = executedPayment.state
                };
                _paymentService.CreateTransactionDetail(paymentModel);
                ViewBag.TransactionIsMade = true;
                return RedirectToAction("UserTodayAppointments", "ProfileController", true);

                //RedirectToAction("PaymentSuccess");
            }
            else
            {
                // Payment failed
                return RedirectToAction("PaymentFailure");
            }
        }


    }
}
