using MentalHealthApp.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthApp.WebApp.Code.Base
{
    public class BaseController : Controller
    {
        protected readonly CurrentUserDto CurrentUser;
    //    protected readonly EditUserDto EditUser;
        public BaseController(ControllerDependencies controllerDependencies) : base()
        {
            CurrentUser = controllerDependencies.CurrentUser;
           // EditUser = controllerDependencies.EditUser;
        }
    }
}
