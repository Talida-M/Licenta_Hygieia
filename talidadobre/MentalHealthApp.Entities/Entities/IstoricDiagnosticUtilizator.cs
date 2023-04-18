using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class IstoricDiagnosticUtilizator : IEntity
    {
        public int Id { get; set; }
        public Guid DiagnosticId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DataDiagnosticare { get; set; }

        public virtual Diagnostic Diagnostic { get; set; } = null!;
        public virtual Pacient Pacient { get; set; } = null!;
    }
}
