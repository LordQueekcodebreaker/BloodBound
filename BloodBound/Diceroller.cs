using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBound
{
    public class Diceroller
    {
        public int Roll()
        {
            Random rnd = new Random();
            return rnd.Next(1,11);
        }
    }
}
