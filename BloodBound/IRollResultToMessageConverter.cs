using BloodBound;

public interface IRollResultToMessageConverter
{
    string ToMessage(RollResultContainer container, int index);
}