using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBound
{
    public class RerollResultContainer
    {
        public RollResultContainer RollResult { get; set; }
        public int HungerIndex { get; set; }
        public string OriginalResult { get; set; }
    }
}
