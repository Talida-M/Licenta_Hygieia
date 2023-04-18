using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.Exceptions
{
    public class AlreadyExistsInDB : Exception
    {
        public string EntityName { get; }

        public AlreadyExistsInDB(string entityName, string errMessage) : base(errMessage)
        {
            EntityName = entityName;
        }
    }
}
