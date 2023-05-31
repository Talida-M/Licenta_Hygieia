using MentalHealthApp.BusinessLogic.Implementation.Account;
using MentalHealthApp.BusinessLogic.Implementation.Admin;
using MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels;
using MentalHealthApp.WebApp.Code.Base;
using MentalHealthApp.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MentalHealthApp.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomePageService _homePageService;
        private readonly UserAccountService _user;
        public HomeController(ILogger<HomeController> logger,
                              HomePageService homePageService,
                              UserAccountService userAccount,
                              ControllerDependencies dependencies) :base(dependencies)
        {
            _logger = logger;
            _homePageService = homePageService;
            _user = userAccount;
        }

        [HttpGet]
        public IActionResult AllConditionsPosts()
        {
            var articlesList = _homePageService.GetAllConditions();
            var model = new ConditionsPostVM()
            {
                ConditionsPostVMs = articlesList.Select(art => new ConditionsPostVM
                {
                    Id = art.Id,
                    ConditionName = art.ConditionName,
                    Description = art.Description,
                    ShortDecsription = art.ShortDecsription,
                    Image = art.Image
                }).ToList()

            };
            return View(model);
        }

        [HttpGet]
        public IActionResult GetConditionPost(Guid id)
        {
            var article = _homePageService.GetCondition(id);
            if(article == null)
            {
                return View("PageNotFound");
            }
            return View("ConditionPost", article);
        }

        public  IActionResult Index()
        {
            var articlesList =  _homePageService.GetAllArticles();
            var conditionList = _homePageService.GetFourConditions();
            if(CurrentUser.isAuthenticated == true)
            {
                var status = _user.GetUserActiveStatus();
                if (status != null)
                {
                    ViewBag.IsActiveStatus = status;
                    ViewData["UserImage"] = _user.GetUserImage();
                }
            }
           
            var model = new HomePageVM()
            {
                TopReads = articlesList.Select(art => new TopReadsVM
                {
                    Id = art.Id,
                    ArticleTile = art.ArticleTile,
                    Author = art.Author,
                    Content = art.Content,
                    ArticleImage = art.ArticleImage
                }).ToList(),
                ConditionsPostVMs = conditionList.Select(condition => new ConditionsPostVM
                {
                    Id = condition.Id,
                    ConditionName = condition.ConditionName,
                    ShortDecsription = condition.ShortDecsription,                   
                    Description = condition.Description,
                    Image = condition.Image
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}