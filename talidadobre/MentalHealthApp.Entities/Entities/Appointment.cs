using MentalHealthApp.Common;
using MentalHealthApp.Entities.Entities;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class Appointment : IEntity
    {
        public int Id { get; set; }
        public Guid SpecialistId { get; set; }
        public Guid UserId { get; set; }
        public string AppointmentDate { get; set; } = null!;
       // public string OraAppointment { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public string? AppointmentStatus { get; set; }

        public virtual Specialist Specialist { get; set; } = null!;
        public virtual Pacient Pacient { get; set; } = null!;
        public virtual PaymentDetails PaymentDetails { get; set; } = null!;
    }
}
