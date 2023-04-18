using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public class ConditionsPost : IEntity
    {
        public Guid Id { get; set; }
        public string ConditionName { get; set; } = null!;
        public string ShortDecsription { get; set ; } = null!;
        public string Description { get; set; } = null!;
        public byte[] Image { get; set; } = null!;
    }
}
