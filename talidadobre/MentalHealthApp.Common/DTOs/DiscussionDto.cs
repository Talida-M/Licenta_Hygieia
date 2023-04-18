using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.DTOs
{
    public class DiscussionDto
    {
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string MessageContent { get; set; } = null!;
        public DateTime CommentDate { get; set; }
    }
}
