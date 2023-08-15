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
        SetRollResult(ListResults, hungerIndex);
        return _resultContainer;
    }

    public RollResultContainer RerollDiceResult(RollResultContainer container, int index)
    {
        var regularDice = container.DiceResult.Take(index).ToArray();
        var hungerDice = container.DiceResult.Skip(index).ToArray();
        var rerolledRegularDice = RerollDice(regularDice);
        var listResult = rerolledRegularDice.Concat(hungerDice).ToArray();
        SetRollResult(listResult, index);
        return _resultContainer;
    }

    private void SetRollResult(int [] listResults, int index)
    {
        _resultContainer.Successes = CalculateSuccesses(listResults);
        _resultContainer.Messy = IsMessy(listResults, index);
        _resultContainer.BeastlyFail = IsBeastlyFail(listResults, index);
        _resultContainer.DiceResult = listResults;
    }

    public int [] RerollDice(int[] vs)
    {
        int count = 0;
        for (int i = 0; i < vs.Length; i++)
        {
            if (vs[i] < 6 && count < 3)
            {
                vs[i] = _rollerService.Roll();
                count++;
            }
        }
        return vs;
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
            if (regularDice.Contains(10) && hungerDice.Contains(10)|| hungerDice.Where(x => x == 10).Count() > 1)
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