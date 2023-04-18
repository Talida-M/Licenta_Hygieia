using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class CameraConferintum :IEntity
    {
        public Guid Id { get; set; }
        public Guid SpecialistId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Data { get; set; }
        public string OraInceput { get; set; } = null!;
        public string OraSfarsit { get; set; } = null!;

        public virtual Specialist Specialist { get; set; } = null!;
        public virtual Pacient Pacient { get; set; } = null!;
    }
}
