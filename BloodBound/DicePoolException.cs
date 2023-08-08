using System.Runtime.Serialization;

namespace BloodBound
{
    [Serializable]
    public class DicePoolException : Exception
    {
        public DicePoolException()
        {
        }

        public DicePoolException(string? message) : base(message)
        {
        }
    }
}