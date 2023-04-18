using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class DiscussionComment : IEntity
    {
        public Guid Id { get; set; }
        public Guid DiscussionId { get; set; }
        public Guid UserId { get; set; }
        public string MessageContent { get; set; } = null!;
        public DateTime CommentDate { get; set; }

        public virtual Discussion Discussion { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;
    }
}
