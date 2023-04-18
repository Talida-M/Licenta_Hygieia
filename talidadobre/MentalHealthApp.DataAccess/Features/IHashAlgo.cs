using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.DataAccess.Features
{
    public interface IHashAlgo
    {
        string CalculateHashValueWithInput(string input);
        bool IsPasswordVerified(string initialHash, string usedSalt, string input);
    }
}
