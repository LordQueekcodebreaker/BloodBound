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
        public void CulculateSuccesses_TwoSuccesinArray_ReturnsTwo()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 6, 4, 2, 8 };
            int expectedResult = 2;

            int result = drc.CalculateSuccesses(vs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CulculateSuccesses_NoSuccesinArray_Returnszero()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 1, 4, 2, 3 };
            int expectedResult = 0;

            int result = drc.CalculateSuccesses(vs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CulculateSuccesses_EmptyArray_Returnszero()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[0];
            int expectedResult = 0;

            int result = drc.CalculateSuccesses(vs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CulculateSuccesses_EvenNumberOfTensInArray_ReturnFour()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 10, 4, 10, 3 };
            int expectedResult = 4;

            int result = drc.CalculateSuccesses(vs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CulculateSuccesses_OddNumberOfTensInArray_ReturnFive()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 10, 4, 10, 3, 10 };
            int expectedResult = 5;

            int result = drc.CalculateSuccesses(vs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CulculateSuccesses_RegularSuccessesAndCrit_ReturnFive()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 10, 4, 10, 3, 8};
            int expectedResult = 5;

            int result = drc.CalculateSuccesses(vs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsMessy_OneTenInHungerpoolAndRegular_ReturnsTrue()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 10, 4, 2, 10 };
            bool expectedResult = true;

            drc.CalculateSuccesses(vs);
            bool result = drc.IsMessy(vs, 2);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsMessy_NoTenInHungerpool_ReturnsFalse()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 10, 10, 2, 8 };
            bool expectedResult = false;

            drc.CalculateSuccesses(vs);
            bool result = drc.IsMessy(vs, 2);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsMessy_NoTenInRegularpool_ReturnsFalse()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 8, 8, 10, 10 };
            bool expectedResult = false;

            drc.CalculateSuccesses(vs);
            bool result = drc.IsMessy(vs, 2);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsMessy_HungerZero_ReturnsFalse()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[] { 8, 8, 10, 10 };
            bool expectedResult = false;

            drc.CalculateSuccesses(vs);
            bool result = drc.IsMessy(vs, 0);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsMessy_DicepoolZero_ReturnsFalse()
        {
            Diceroller dr = new Diceroller();
            DiceRollController drc = new DiceRollController(dr);
            int[] vs = new int[0];
            bool expectedResult = false;

            drc.CalculateSuccesses(vs);
            bool result = drc.IsMessy(vs, 0);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

    }
}
