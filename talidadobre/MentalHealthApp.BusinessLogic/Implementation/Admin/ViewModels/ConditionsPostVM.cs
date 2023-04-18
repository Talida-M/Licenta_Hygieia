using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class ConditionsPostVM
    {
        public Guid Id { get; set; }
        public string ConditionName { get; set; } = null!;
        public string ShortDecsription { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile ImageForm { get; set; } = null!;
        public byte[] Image { get; set; } = null!;
        public List<ConditionsPostVM> ConditionsPostVMs { get; set; } = null!;
    } 
}
