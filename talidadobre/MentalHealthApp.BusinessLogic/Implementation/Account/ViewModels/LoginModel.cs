using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AreCredentialsInvalid { get; set; }
    }
}
