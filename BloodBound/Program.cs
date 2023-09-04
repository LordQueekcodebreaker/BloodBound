using BloodBound;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class Program
{
    private DiscordSocketClient _client { get; set; }
    private IRollerService _rollService { get; set; }
    private DiceRollController _rollController { get; set; }
    public IRollResultToMessageConverter _resultConverter { get; set; }
    public Dictionary<string, RerollResultContainer> _availableRerolls { get; set; }
    private string _token { get; set; }


    public static Task Main(string[] args)
    { 
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        DotEnv.Load(dotenv);

        return new Program().MainAsync();
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task MainAsync()
    {
        _token = Environment.GetEnvironmentVariable("DISCORDTOKEN");
        _rollService = new Diceroller();
        _rollController = new DiceRollController(_rollService);
        _availableRerolls = new Dictionary<string, RerollResultContainer>();
        _resultConverter = new RollResultEmbedBuilder();
        await SetClient();
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    public async Task Client_Ready()
    {
        List<ApplicationCommandProperties> applicationCommandProperties = new();

        var globalCommand = new SlashCommandBuilder();
        globalCommand.WithName("first-command");
        globalCommand.WithDescription("This my first global Slash Command");
        applicationCommandProperties.Add(globalCommand.Build());

        var globalRollCommand = new SlashCommandBuilder();
        globalRollCommand.WithName("roll");
        globalRollCommand.WithDescription("Gives a single D10 result");
        applicationCommandProperties.Add(globalRollCommand.Build());

        var globalRollPoolCommand = new SlashCommandBuilder();
        globalRollPoolCommand.WithName("roll-pool");
        globalRollPoolCommand.WithDescription("Gives a set of D10 results");
        globalRollPoolCommand.AddOption("pool", ApplicationCommandOptionType.Integer,"amount in the dicepool", isRequired: true, maxValue:21, minValue:1);
        globalRollPoolCommand.AddOption(new SlashCommandOptionBuilder().WithName("hunger").WithDescription("amount of hunger").WithRequired(true)
            .AddChoice("0", 0)
            .AddChoice("1", 1)
            .AddChoice("2", 2)
            .AddChoice("3", 3)
            .AddChoice("4", 4)
            .AddChoice("5", 5)
            .WithType(ApplicationCommandOptionType.Integer));

        var globalRouseCommand = new SlashCommandBuilder();
        globalRouseCommand.WithName("rouse");
        globalRouseCommand.WithDescription("determines hunger gain");
        applicationCommandProperties.Add(globalRouseCommand.Build());


        applicationCommandProperties.Add(globalRollPoolCommand.Build());

        try
        {
            await _client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray());
        }
        catch (ApplicationCommandException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
    private async Task SetClient()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.ButtonExecuted += ButtonHandler;
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "roll":
                int value = _rollService.Roll();
                await command.RespondAsync($"you rolled a {value.ToString()}");
                return;

            case "roll-pool":
                var optionsArray =command.Data.Options.ToArray();
                var dicePool = Convert.ToInt32(optionsArray[0].Value);
                var hunger = Convert.ToInt32(optionsArray[1].Value);
                RollResultContainer result = _rollController.DetermineResult(dicePool, hunger);
                var message = _resultConverter.ToMessage(result, dicePool - hunger);
                string name = command.User.Username;
                if (!_availableRerolls.ContainsKey(name))
                {
                    _availableRerolls.Add(name, new RerollResultContainer() { HungerIndex = dicePool - hunger, RollResult = result, OriginalResult = $"{message.Title}({result.Successes})" });
                }
                _availableRerolls[name] = new RerollResultContainer() { HungerIndex = dicePool - hunger, RollResult = result, OriginalResult = $"{message.Title}({result.Successes})" };
                var rerollButton = GetButton(result, hunger);
                await command.RespondAsync(embed: message.Build(), components: rerollButton.Build());
                return;

            case "rouse":
                int roll = _rollService.Roll();
                var rouseMessage = _resultConverter.ToRouseMessage(roll);
                rouseMessage.WithAuthor(command.User);
                await command.RespondAsync(embed: rouseMessage.Build());
                return;
        }
        await command.RespondAsync($"You executed {command.Data.Name}");
    }

    private async Task ButtonHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "regular-reroll":
                var user = component.User.Username;
                if (!_availableRerolls.ContainsKey(user))
                {
                    await component.RespondAsync($"{component.User.Mention} Has no permission to reroll",ephemeral: true);
                    break;
                }
                var result =_availableRerolls[user];
                _availableRerolls.Remove(user);
                result.RollResult = _rollController.RerollDiceResult(result.RollResult, result.HungerIndex);
                var message = _resultConverter.ToRerollMessage(result, result.HungerIndex);
                var msg = await component.Channel.GetMessagesAsync(1).FirstAsync();
                await component.Channel.DeleteMessageAsync(msg.First().Id);
                message.WithAuthor(component.User);
                await component.RespondAsync(embed: message.Build());
                break;
        }
    }

    private ComponentBuilder GetButton(RollResultContainer container, int hunger)
    {
        var builder = new ComponentBuilder();
        if (container.DiceResult.Length <= hunger)
        {
            return builder.WithButton("Reroll", "regular-reroll", disabled: true);
        }
        return builder.WithButton("Reroll", "regular-reroll", disabled: false);
    }
}