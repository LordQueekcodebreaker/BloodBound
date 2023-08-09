using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBound
{
    //public enum DiceState {Crit, BeastlyFail, MessyCrit  }

    public class RollResultContainer
    {
        public int[]? DiceResult { get; set; }
        public int Successes { get; set; } = 0;
        public Boolean Crit { get; set; }
        public Boolean Messy { get; set; }
        public Boolean BeastlyFail { get; set; }





    }
}
