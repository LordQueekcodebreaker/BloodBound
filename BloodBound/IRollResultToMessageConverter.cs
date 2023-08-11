using BloodBound;
using Discord;

public interface IRollResultToMessageConverter
{
    EmbedBuilder ToMessage(RollResultContainer container, int index);
}