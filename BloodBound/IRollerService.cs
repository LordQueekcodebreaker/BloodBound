namespace BloodBound
{
    public interface IRollerService
    {
        int Roll();
        int[] Roll(int diceCount);
    }
}