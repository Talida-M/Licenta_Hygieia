using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.UserJournals.ViewModel
{
    public class JournalPageVM
    {
        public Guid Id { get; set; }
        public DateTime PageDate { get; set; }
        public string Content { get; set; } = null!;
        public bool IsPublic { get; set; } = false;
        
    }
}
