using NUnit.Framework;
using BloodBound;

namespace BloodBoundTestProject
{
    public class Tests
    {
 
        [Test]
        public void Roll_ReturnsResult()
        {
            Diceroller dr = new Diceroller();
            var result = dr.Roll();
            Assert.That(result, Is.GreaterThan(0));

        }
    }
}