using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
     public partial class DoctorCV : IEntity
    {
        [Key]
        public int CVId { get; set; }

        public Guid SpecialistId { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public byte[] DataFiles { get; set; }
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("SpecialistId")]
        public virtual Specialist Specialist { get; set; } = null!;
    }
}
