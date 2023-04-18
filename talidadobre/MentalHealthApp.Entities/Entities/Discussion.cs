using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class Discussion : IEntity
    {
        public Discussion()
        {
            DiscussionComments = new HashSet<DiscussionComment>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string MessageContent { get; set; } = null!;
        public DateTime CommentDate { get; set; }

        public virtual IdentityUser User { get; set; } = null!;
        public virtual ICollection<DiscussionComment> DiscussionComments { get; set; }
    }
}
