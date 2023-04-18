using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public class TopReads : IEntity
    {
        public Guid Id { get; set; }
        public string ArticleTile { get; set; } = null!;
        public string? Author { get; set; }
        public string Content { get; set; } = null!;
        public byte[] ArticleImage { get; set; } = null!;

    }
}
