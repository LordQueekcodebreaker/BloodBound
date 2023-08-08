using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBound
{
    public class Diceroller : IRollerService
    {
        public int Roll()
        {
            Random rnd = new Random();
            return rnd.Next(1,11);
        }

        public int[] Roll(int diceCount)
        {
            if (diceCount <= 0)
                throw new DicePoolException("DicePool Requires at least one dice");

            int[] result = new int [diceCount];
            for (int i = 0; i < diceCount; i++)
            {
                result[i] = Roll();
            }
            return result;
        }
    }
}
