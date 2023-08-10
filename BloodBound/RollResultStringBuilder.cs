using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBound
{
    public class RollResultStringBuilder : IRollResultToMessageConverter
    {
        public string ToMessage(RollResultContainer container, int index)
        {
            string dicePool = String.Join(", ", ToStringArray(container.DiceResult, index));
            string title = GetTitle(container);
            string successes = GetSuccesses(container);
            return $"{title}\n[{dicePool}]\n{successes}";
        }

        private string GetSuccesses(RollResultContainer container)
        {
            string result = $"You Rolled {container.Successes}";
            if (container.Successes == 1)
            {
                return result += " succes";
                
            }
            return result += " succeses";
        }

        public string GetTitle(RollResultContainer container)
        {
            if (container.Crit)
            {
                if (container.Messy)
                {
                    return "Messy Success!";
                }
                return "Critical Success!";
            }
            else if (container.BeastlyFail)
            {
                return "Beastial Failure!";
            }
            else { 
                return "Success";
            }
        }

        private string[] ToStringArray(int [] diceResult, int index)
        {
            string[] regulardice = diceResult.Take(index).Select(x => x.ToString()).ToArray();
            string[] hungerdice = diceResult.Skip(index).Select(x => $"__{x.ToString()}__").ToArray();
            string[] dicepool = regulardice.Concat(hungerdice).ToArray();
            return dicepool;
        } 
    }
}
