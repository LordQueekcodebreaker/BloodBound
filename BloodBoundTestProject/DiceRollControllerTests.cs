using BloodBound;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBoundTestProject
{
    public class DiceRollControllerTests
    {
        [Test]
        public void Roll_ReturnsResult()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            
            drc.TakeRegularDice()

            Assert.That(result, Is.GreaterThan(0));
        }
    }
}
