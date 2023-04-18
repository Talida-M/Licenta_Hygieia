using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Reviews.ViewModel
{
    public class DoctorReviewVM
    {
        public string PacientName { get; set; } = null!;
        public string Review { get; set; } = null!;
        public DateTime CommentDate { get; set; }
        public int StarsNumber { get; set; } = 0;
        public byte[]? UserImage { get; set; }
    }
}
