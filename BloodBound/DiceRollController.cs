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
        _resultContainer = new RollResultContainer();
        var ListResults = _rollerService.Roll(dicePool);
        var hungerIndex = dicePool - hunger;
        _resultContainer.Successes = CalculateSuccesses(ListResults);
        _resultContainer.Messy = IsMessy(ListResults, hungerIndex);
        _resultContainer.BeastlyFail = IsBeastlyFail(ListResults, hungerIndex);
        _resultContainer.DiceResult = ListResults;
        
        return _resultContainer;
    }


    public int CalculateSuccesses(int[] diceArray)
    {
        int sum = diceArray.Where(x => x >= 6).Count(); 
        int tenCount = diceArray.Where(x => x == 10).Count();
        if (tenCount >=2)
        {
            _resultContainer.Crit = true;
            sum += CalculateCritSuccesses(tenCount);
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
                _resultContainer.Messy = true;
                return true;
            }
        }
        return false;
    }

    public bool IsBeastlyFail(int[] Diceresult, int index)
    {
        var hungerPool = Diceresult.Skip(index).ToArray();
        var succesCount = Diceresult.Where(x => x >= 6).Count();
        if (hungerPool.Contains(1) && succesCount == 0)
        {
            _resultContainer.BeastlyFail = true;
            return true;
        }
        return false;
    }

    Func<double, int> CalculateCritSuccesses = (tenCount) => Convert.ToInt32( Math.Floor(tenCount / 2) * 2);
}