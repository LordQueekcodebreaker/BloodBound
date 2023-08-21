using BloodBound;
using Discord;

public interface IRollResultToMessageConverter
{
    EmbedBuilder ToMessage(RollResultContainer container, int index);
    EmbedBuilder ToRerollMessage(RerollResultContainer container, int index);
    EmbedBuilder ToRouseMessage(int value);
}