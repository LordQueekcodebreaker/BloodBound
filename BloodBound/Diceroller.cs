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

        public int[] RollPool(int v)
        {
            if (v <= 0)
                throw new DicePoolException("DicePool Requires at least one dice");

            int[] result = new int [v];
            for (int i = 0; i < v; i++)
            {
                result[i] = Roll();
            }
            return result;
        }
    }
}
