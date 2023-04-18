using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class Simptome : IEntity
    {
        public Simptome()
        {
            Diagnostics = new HashSet<Diagnostic>();
            Pacients = new HashSet<Pacient>();
        }

        public Guid Id { get; set; }
        public string Denumire { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Diagnostic> Diagnostics { get; set; }
        public virtual ICollection<Pacient> Pacients { get; set; }
    }
}
