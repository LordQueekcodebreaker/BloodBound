using BloodBound;

public class DiceRollController
{
    private IRollerService _rollerService { get; set; }
    public RollResultContainer _resultContainer { get; set; }

    public DiceRollController(IRollerService rollerService)
    {
        _rollerService = rollerService;
        _resultContainer = new RollResultContainer();
    }

    public RollResultContainer DetermineResult(int dicePool, int hunger)
    {
        RollResultContainer resultContainer = new RollResultContainer();
        var ListResults = _rollerService.Roll(dicePool);
        //var regularDice = TakeRegularDice(ListResults, dicePool - hunger);
        CalculateSuccesses(ListResults);
        return resultContainer;

    }

    public void CalculateSuccesses(int[] regularDice)
    {
   
    }

    //public Func<int[],int, int[]> TakeRegularDice = (array, index) => array.Take(index).ToArray();
}