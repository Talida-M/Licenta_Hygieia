using MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.UserJournals.ViewModel
{
	public class JournalVM
	{
        public string? Content { get; set; }
        public bool IsPublic { get; set; } = false;
        public List<JournalPageVM>? JournalPages { get; set; }

    }
}
