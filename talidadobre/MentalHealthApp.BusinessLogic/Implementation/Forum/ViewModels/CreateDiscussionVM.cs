using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels
{
    public class CreateDiscussionVM
    {
        //public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string MessageContent { get; set; } = null!;
        public List<DiscussionVM> DiscussionVMs { get; set; } = null!;
    }
}
