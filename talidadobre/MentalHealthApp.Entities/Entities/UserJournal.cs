using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public partial class UserJournal : IEntity
    {
        public Guid Id { get; set; }
        public Guid PacientId { get; set; }
        public DateTime PageDate { get; set; }
        public string Content { get; set; } = null!;
        public bool IsPublic { get; set; } = false;
        public virtual Pacient Pacient { get; set; } = null!;
    }
}
