using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Reviews.ViewModel
{
    public class NewReviewVM
    {
        public Guid Id { get; set; }
        public Guid PacientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Review { get; set; } = null!;
        public DateTime CommentDate { get; set; }
        public int StarsNumber { get; set; } = 0;
    }
}
