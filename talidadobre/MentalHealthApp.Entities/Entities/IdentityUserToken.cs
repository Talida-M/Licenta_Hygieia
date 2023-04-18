using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class IdentityUserToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TokenValue { get; set; } = null!;
        public string? RefreshTokenValue { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsTokenRevoked { get; set; }

        public virtual IdentityUser User { get; set; } = null!;
    }
}
