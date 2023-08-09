using BloodBound;

public class DiceRollController
{
    private IRollerService _rollerService { get; set; }
    private RollResultContainer _resultContainer { get; set; }

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
        _resultContainer.Successes = CalculateSuccesses(ListResults);
        _resultContainer.Messy = IsMessy(ListResults, dicePool - hunger); 
        _resultContainer.DiceResult = ListResults;
        
        return resultContainer;

    }

    public int CalculateSuccesses(int[] diceArray)
    {
        double tenCount = 0;
        int sum = 0;
        for (int i = 0; i < diceArray.Length; i++)
        {
            if (diceArray[i] >= 6)
            {
                if (diceArray[i] == 10)
                {
                    tenCount++;
                }
                sum++;
            }
        }
        if (tenCount >=2)
        {
            sum += CalculateCritSuccesses(tenCount);
            _resultContainer.Crit = true;
        }
       
        return sum;
    }

    public bool IsMessy(int[] Diceresult, int index)
    {
        if (_resultContainer.Crit)
        {
            var regularDice = Diceresult.Take(index).ToArray();
            var hungerDice = Diceresult.Skip(index).ToArray();
            if (regularDice.Contains(10) && hungerDice.Contains(10))
            {
                return true;
            }
        }
        return false;
    }


    Func<int[],int, int[]> TakeRegularDice = (array, index) => array.Take(index).ToArray();
    Func<double, int> CalculateCritSuccesses = (tenCount) => Convert.ToInt32( Math.Floor(tenCount / 2) * 2);
}