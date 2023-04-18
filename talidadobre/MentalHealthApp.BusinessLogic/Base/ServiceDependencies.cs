using MentalHealthApp.Common.DTOs;
using MentalHealthApp.DataAccess;
using MentalHealthApp.DataAccess.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Base
{
    public class ServiceDependencies
    {
        public UnitOfWork UnitOfWork { get; set; }
        public CurrentUserDto CurrentUser { get; set; }
        //public EditUserDto EditUser { get; set; }
      //  public HashAlg HashAlg { get; set; }




        public ServiceDependencies(UnitOfWork unitOfWork, CurrentUserDto currentUser)//, HashAlg hashAlg)
        {
            UnitOfWork = unitOfWork;
            CurrentUser = currentUser;
       //     EditUser = editUser;
                //   HashAlg = hashAlg;
        }
    }
}
