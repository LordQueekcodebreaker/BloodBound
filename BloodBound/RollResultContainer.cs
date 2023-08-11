using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBound
{
    public class RollResultContainer
    {
        public int[] DiceResult { get; set; }
        public int Successes { get; set; }
        public Boolean Crit { get; set; }
        public Boolean Messy { get; set; }
        public Boolean BeastlyFail { get; set; }
    }
}
