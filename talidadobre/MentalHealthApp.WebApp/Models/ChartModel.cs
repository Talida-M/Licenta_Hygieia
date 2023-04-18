using System.Runtime.Serialization;
using System.Xml.Linq;

namespace MentalHealthApp.WebApp.Models
{
    [DataContract]
    public class ChartModel
    {
        public ChartModel(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        [DataMember(Name = "label")]
        public string Label = "";

        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}
