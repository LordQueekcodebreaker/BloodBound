using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBound
{
    public class RollResultEmbedBuilder : IRollResultToMessageConverter
    {
        public EmbedBuilder _message { get; set; }

        public EmbedBuilder ToMessage(RollResultContainer container, int index)
        {
            _message = new EmbedBuilder();
            string[] dicepool = ToEmoteArray(container.DiceResult, index);
            string EmoteResult = String.Join(" ", dicepool);
            _message.Title = GetTitle(container);
            _message.AddField("Roll Result", EmoteResult, false);
            _message.AddField("Success:", container.Successes, true);
            _message.AddField("Dice:", container.DiceResult.Length, true);
            _message.AddField("Hunger:", container.DiceResult.Length - index, true);
            return _message;
        }

        public EmbedBuilder ToRerollMessage(RerollResultContainer reroll, int index)
        {
            _message = ToMessage(reroll.RollResult, index);
            _message.Title += " • Rerolled Failures";
            _message.AddField("Original Result",$"```{reroll.OriginalResult}```", false);
            return _message;
        }
        public EmbedBuilder ToRouseMessage(int value)
        {
            _message = new EmbedBuilder();
            bool success = (value >= 6 ? true : false);
            _message.Title = (success ? "Rouse Success" : "Rouse Failure");
            _message.Description = (success ? "No hunger gained" : "Gained hunger!");
            _message.ThumbnailUrl = (success ? "https://assets.inconnu.app/hunger-unfilled.webp" : "https://assets.inconnu.app/hunger-filled.webp");
            return _message;
        }

        private string GetTitle(RollResultContainer container)
        {
            if (container.Crit)
            {
                if (container.Messy)
                {
                    _message.Color = Color.Red;
                    return "Messy Success";
                }
                _message.Color = Color.Blue;
                return "Critical Success";
            }
            else if (container.BeastlyFail)
            {
                _message.Color = Color.DarkRed;
                return "Beastial Failure";
            }
            else
            {
                _message.Color = Color.LightGrey;
                return "Success";
            }
        }

        private string[] ToEmoteArray(int[] diceResult, int index)
        {
            string[] regulardice = diceResult.Take(index).Select(x => ToRegularEmote(x)).ToArray();
            string[] hungerdice = diceResult.Skip(index).Select(x => ToHungerEmote(x)).ToArray();
            string[] dicepool = regulardice.Concat(hungerdice).ToArray();
            return dicepool;
        }

        private string ToRegularEmote(int value)
        {
            switch (value)
            {
                case < 6:
                    return "<:vtmFail:955537215893688390>";
                case <= 9:
                    return "<:vtmSuccess:955537199363948545>";
                case 10:
                    return "<:vtmCriticalSuccess:955537153960591360>";
            }
            return "<:mtaBeastlyFail:1087016470741008495>";
        }

        private string ToHungerEmote(int value)
        {
            switch (value)
            {
                case 1:
                    return "<:vtmBeastlyFail:955537229776842783>";
                case < 6:
                    return "<:vtmBeastlyFailure:955537260407832636>";
                case <= 9:
                    return "<:vtmBeastlySuccess:955537240757534752>";
                case 10:
                    return "<:vtmBeastlyCritical:955537251184562196>";
            }
            return "<:mtaBeastlyFail:1087016470741008495>";
        }
    }
}
