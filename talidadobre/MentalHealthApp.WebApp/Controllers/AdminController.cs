using AutoMapper;
using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Admin;
using MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using System.Collections;
using System.Text.Json.Nodes;
using MentalHealthApp.WebApp.Code.ExtensionMethods;
using MentalHealthApp.WebApp.Models;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;

namespace MentalHealthApp.WebApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly UserAccountService _userAccountService;
        private readonly UserProfileService _userProfileService;
        private readonly DoctorAccountService _doctorAccountService;
        private readonly ProgramareService _programareService;
        private readonly IMapper _mapper;
        private readonly HomePageService _homePageService;
        private readonly ChartService _chartService;
        private static readonly HttpClient client = new HttpClient();
        public AdminController(ControllerDependencies dependencies,
                               UserAccountService userAccountService,
                               UserProfileService userProfileService,
                               DoctorAccountService doctorAccountService,
                               ProgramareService programareService,
                               HomePageService homePageService,
                               IMapper mapper,
                               ChartService chart)
            : base(dependencies)
        {
            _userAccountService = userAccountService;
            _userProfileService = userProfileService;
            _doctorAccountService = doctorAccountService;
            _programareService = programareService;
            _mapper = mapper;
            _homePageService = homePageService;
            _chartService = chart;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UsersDetails(int page)
        {
            int pageSize = 10;
            var model = _userAccountService.GetPageWithUsers(pageSize * (page), pageSize);
            return View("UsersDetails", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UsersPage(int page)
        {
            int pageSize = 10;
            var editUserDtos = _userAccountService.GetPageWithUsers(pageSize * (page), pageSize);
            var model = (editUserDtos, page);
            return View("UsersDetails", model);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult DeleteUser(string email)
        {
            var model = _userAccountService.DeleteUser(email);

            return Ok();

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult DeleteDoctor(string email)
        {
            var model = _doctorAccountService.DeleteDoctor(email);

            return Ok();

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditUser(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userDto = _mapper.Map<EditUserDto>(model);
            _userProfileService.EditUserInfo(userDto);
            return RedirectToAction("UsersDetails");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string email)
        {
            var userDto = await _userProfileService.GetUserByEmail(email);
            var model = _mapper.Map<UserVM>(userDto);
            return View(model);
        }

        //Home Page Management
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddConditionPost(ConditionsPostVM model)
        {
            _homePageService.AddCondition(model);
            return RedirectToAction("ConditionsManagement");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddArticle(TopReadsVM model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction("ArticlesManagement");
            //}
            _homePageService.AddArticle(model);
            return RedirectToAction("ArticlesManagement");

        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public IActionResult ArticlesManagement(int page)
        //{
        //    int pageSize = 4;
        //   // var articlesList = _homePageService.GetAllArticles();
        //    var articlesList = _homePageService.GetPageArticles(pageSize * (page), pageSize);
        //    var model = new TopReadsVM()
        //    {
        //        TopReads = articlesList.Select(art => new TopReadsVM
        //        {
        //            Id = art.Id,
        //            ArticleTile = art.ArticleTile,
        //            Author = art.Author,
        //            Content = art.Content
        //        }).ToList(),

        //    };
        //    return View(model);
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ArticlesManagement(int page)
        {
            if (page < 0)
            {
                page = 0;
            }
            int pageSize = 4;
            // var articlesList = _homePageService.GetAllArticles();
            var articlesList = _homePageService.GetPageArticles(pageSize * (page), pageSize);
            var topReadsVM = new TopReadsVM()
            {
                TopReads = articlesList.Select(art => new TopReadsVM
                {
                    Id = art.Id,
                    ArticleTile = art.ArticleTile,
                    Author = art.Author,
                    Content = art.Content
                }).ToList(),

            };
            var model = (topReadsVM, page);
            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public IActionResult ConditionsManagement(int page)
        //{
        //    int pageSize = 4;
        //    // var articlesList = _homePageService.GetAllConditions();
        //    var articlesList = _homePageService.GetPageConditions(pageSize * (page), pageSize);
        //    var conditionsPostVMs = new ConditionsPostVM()
        //    {
        //        ConditionsPostVMs = articlesList.Select(art => new ConditionsPostVM
        //        {
        //            Id = art.Id,
        //            ConditionName = art.ConditionName,
        //            Description = art.Description,
        //            ShortDecsription = art.ShortDecsription
        //        }).ToList()

        //    };
        //    var model = (conditionsPostVMs, page);
        //    return View(model);
        //}
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ConditionsManagement(int page)
        {
            if (page < 0)
            {
                page = 0;
            }
            int pageSize = 4;
            // var articlesList = _homePageService.GetAllConditions();
            var articlesList = _homePageService.GetPageConditions(pageSize * (page), pageSize);
            var conditionsPostvm = new ConditionsPostVM()
            {
                ConditionsPostVMs = articlesList.Select(art => new ConditionsPostVM
                {
                    Id = art.Id,
                    ConditionName = art.ConditionName,
                    Description = art.Description,
                    ShortDecsription = art.ShortDecsription
                }).ToList()

            };
            var model = (conditionsPostvm, page);
            return View(model);
        }


        //// Doctors
        [Authorize(Roles = "Admin")]
        [HttpGet]
        //public IActionResult DoctorsPage()
        //{
        //    var model = _doctorAccountService.GetAllDoctors();
        //    return View("DoctorsPage", model);
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DoctorsPage(int page)
        {
            int pageSize = 3;
            var editDoctorDtos = _doctorAccountService.GetPageDoctors(pageSize * (page), pageSize);
            var model = (editDoctorDtos, page);
            return View("DoctorsPage", model);
        }


        [HttpGet]
        public IActionResult RegisterDoctor()
        {

            //HttpResponseMessage response = await client.GetAsync("https://regmed.cmr.ro/registrul-medicilor");
            //response.EnsureSuccessStatusCode();

            // var status = responseBody.
            var model = new RegisterDoctorModel();
            return View("RegisterDoctor", model);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(RegisterDoctorModel model)
        {
            if (model == null)
            {
                return View("PageNotFound");
            }
            //string responseBody = await client.GetStringAsync("https://regmed.cmr.ro/api/v2/public/medic/cautare/" + model.LastName + model.FirstName);
            HttpResponseMessage response = await client.GetAsync("https://regmed.cmr.ro/api/v2/public/medic/cautare/" + model.LastName + model.FirstName);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = (JObject)JsonConvert.DeserializeObject(responseBody);
            var responseData = responseJson["data"];
            if (responseData["results"][0]["status"].Value<string>() != null)
            {
                for (int i = 0; i < responseData["total"].Value<int>(); i++)
                {

                    var judet = responseData["results"][i]["judet"].Value<string>();
                    var status = responseData["results"][i]["status"].Value<string>();
                    var isPsihiatru = (responseData["results"][i]["specialitati"].Value<dynamic>() as JArray)
                        .Select(s => (string)s.Value<dynamic>().nume)
                        .Any(s => s.ToLower() == "psihiatrie");
                    if (status == "Activ" && judet.NormalizeString().Equals(model.County))
                    {
                        _doctorAccountService.RegisterNewDoctor(model);
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View("Status is not active, he can not work in present as a doctor");
            }
            else
            {
                return View("Error,doctor does not exist");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditDoctor(DoctorVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userDto = _mapper.Map<EditDoctorDto>(model);
            _doctorAccountService.EditDoctorInfo(userDto);
            return RedirectToAction("DoctorsPage");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditDoctor(string email)
        {
            var userDto = await _doctorAccountService.GetDoctorByEmail(email);
            var model = _mapper.Map<DoctorVM>(userDto);
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AppointmentsDetails(int page)
        {
            int pageSize = 10;
            if (page < 0)
            {
                page = 0;
            }
            var list = _programareService.GetDoctorsName();
            var appointments = _programareService.GetOnePageAppointmentsAdmin(pageSize * (page), pageSize);
            if (appointments.Count() == 0)
            {
                ModelState.AddModelError(nameof(ListAppointmentsModel), "No appointments to show.");
            }

            var appointmentsModel = new ListAppointmentsModel()
            {
                DoctorNameModels = list.Select(app => new DoctorNameModel
                {
                    SpecialistId = app.SpecialistId,
                    DoctorName = app.DoctorName
                }).ToList(),
                AppointmentsAdminModels = appointments.Select(app => new AppointmentsAdminModel
                {
                    Id = app.Id,
                    SpecialistId = app.SpecialistId,
                    DoctorName = app.DoctorName,
                    UserId = app.UserId,
                    UserName = app.UserName,
                    AppointmentDate = app.AppointmentDate,
                    AppointmentType = app.AppointmentType,
                    AppointmentStatus = app.AppointmentStatus
                }).ToList()
            };
            var model = (appointmentsModel, page);
            return View("AppointmentsDetails", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult PageAppointmentsDetails(int page)
        {
            int pageSize = 10;
            var list = _programareService.GetDoctorsName();
            var appointments = _programareService.GetOnePageAppointmentsAdmin(pageSize * (page), pageSize);
            var result = appointments.Select(app => new AppointmentsAdminModel
            {
                Id = app.Id,
                SpecialistId = app.SpecialistId,
                DoctorName = app.DoctorName,
                UserId = app.UserId,
                UserName = app.UserName,
                AppointmentDate = app.AppointmentDate,
                AppointmentType = app.AppointmentType,
                AppointmentStatus = app.AppointmentStatus
            }).ToList();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ViewForumFirstPage()
        {
            return RedirectToAction("ViewForumFirstPage", "Forum");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAppointment(int id)
        {

            var model = await _programareService.DeleteAppointment(id);
            return Ok(model.Id);


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditAppointmentTime(int id, string newTime)
        {
            var result = _programareService.EditTime(id, newTime);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DoctorsCharts()
        {

            List<ChartModel> data1 = new List<ChartModel>();
            var result1 = _chartService.GetRatingInfoChartData();
            foreach (var res in result1)
            {
                data1.Add(new ChartModel(res.Name, res.Info));

            }

            ViewBag.RatingInf = JsonConvert.SerializeObject(data1);


            List<ChartModel> data2 = new List<ChartModel>();
            var result2 = _chartService.GetAppointmentsInfoChartData();
            foreach (var res in result2)
            {
                data2.Add(new ChartModel(res.Name, res.Info));

            }
            ViewBag.AppointmentsInfo = JsonConvert.SerializeObject(data2);

            var result3 = _chartService.GetSpecialistsList();

            var specialistsList = result3.Select(app => new DoctorChartInfoModel
            {
                Name = app.Name,
                Email = app.Email,
                Image = app.Image,
            }).ToList();

            ViewBag.SpecialistsList = specialistsList;


            //List<ChartModel> data3 = new List<ChartModel>();
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetIndividualSpecialistCharts(string email, bool type)
        {
            List<object> data = new List<object>();
            var response = new Dictionary<string, int>();
            if (type == true)
            {
                 response =  _chartService.GetOnlineAppointmentsInfoChartData(email);
                
            }
            else
            {
                response = _chartService.GetF2FAppointmentsInfoChartData(email);
            }
            foreach (var res in response)
            {
                var cheie = res.Key.Split('_');
                var obj = new
                {
                    label = cheie[0] + ' ' + cheie[1],
                    y = res.Value
                };
                data.Add(obj);
            }
           
            return Ok(data);
        }

    }
}
