using NUnit.Framework;
using BloodBound;

namespace BloodBoundTestProject
{
    public class DiceRollerTests
    {
 
        [Test]
        public void Roll_ReturnsResult()
        {
            Diceroller dr = new Diceroller();
            var result = dr.Roll();
            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void RollPool_TwoInput_ReturnsTwoLenghtArray()
        {
            Diceroller dr = new Diceroller();
            int[] result = dr.Roll(2);
            Assert.That(result.Length, Is.EqualTo(2));
        }

        [Test]
        public void RollPool_ThreeInput_ReturnsResultGreaterThanZero()
        {
            Diceroller dr = new Diceroller();
            int[] result = dr.Roll(2);
            Assert.That(result[1], Is.GreaterThan(0));
        }

        [Test]
        public void RollPool_ThreeInput_ReturnsResultlessThanEleven()
        {
            Diceroller dr = new Diceroller();
            int[] result = dr.Roll(2);
            Assert.That(result[1], Is.LessThan(11));
        }

        [Test]
        public void RollPool_ZeroInput_ThrowsDicePoolException()
        {
            Diceroller dr = new Diceroller();
            Assert.Throws<DicePoolException>(() => dr.Roll(0));
        }

    }
}