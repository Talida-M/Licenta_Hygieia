using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class Diagnostic : IEntity
    {
        public Diagnostic()
        {
            IstoricDiagnosticUtilizators = new HashSet<IstoricDiagnosticUtilizator>();
            Simptomes = new HashSet<Simptome>();
        }

        public Guid Id { get; set; }
        public string Denumire { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<IstoricDiagnosticUtilizator> IstoricDiagnosticUtilizators { get; set; }

        public virtual ICollection<Simptome> Simptomes { get; set; }
    }
}
