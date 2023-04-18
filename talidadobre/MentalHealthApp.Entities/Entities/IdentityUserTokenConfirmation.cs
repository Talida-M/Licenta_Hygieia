using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class IdentityUserTokenConfirmation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public byte ConfirmationTypeId { get; set; }
        public string ConfirmationToken { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        public virtual IdentityUser User { get; set; } = null!;
    }
}
