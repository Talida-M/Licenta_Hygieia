using MentalHealthApp.Common.DTOs;

namespace MentalHealthApp.WebApp.Code.Base
{
    public class ControllerDependencies
    {
        public CurrentUserDto CurrentUser { get; set; }
     //   public EditUserDto EditUser { get; set; }


        public ControllerDependencies(CurrentUserDto currentUser)
        {
            CurrentUser = currentUser;
          //  EditUser = editUser;
        }
    }
}
