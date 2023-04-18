using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class IdentityRole :IEntity
    {
        public IdentityRole()
        {
            Users = new HashSet<IdentityUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<IdentityUser> Users { get; set; }
    }
}
