using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public partial class DoctorReviews : IEntity
    {
        public Guid Id { get; set; }
        public Guid PacientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Review { get; set; } = null!;
        public DateTime CommentDate { get; set; }
        public int StarsNumber { get; set; } = 0;
        public virtual Specialist Specialist { get; set; } = null!;
        public virtual Pacient Pacient { get; set; } = null!;
    }
}
