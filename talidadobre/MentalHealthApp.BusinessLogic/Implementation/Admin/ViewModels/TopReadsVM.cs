using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class TopReadsVM
    {
        public Guid Id { get; set; }
        public string ArticleTile { get; set; } = null!;
        public string? Author { get; set; }
        public string Content { get; set; } = null!;
        public IFormFile ImageForm { get; set; } = null!;
        public byte[] ArticleImage { get; set; } = null!;
        public List<TopReadsVM> TopReads { get; set; } = null!;
    }
}
