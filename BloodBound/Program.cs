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
    public Dictionary<string, RollResultContainer> _availableRerolls { get; set; }
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
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        // _client.ButtonExecuted += ButtonHandler;
        _token = Environment.GetEnvironmentVariable("DISCORDTOKEN");
        _rollService = new Diceroller();
        _rollController = new DiceRollController(_rollService);
        _availableRerolls = new Dictionary<string, RollResultContainer>();
        _resultConverter = new RollResultEmbedBuilder();
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
                string name = command.User.Username;
                if (_availableRerolls.ContainsKey(name))
                {
                    _availableRerolls.Remove(name);
                }
                _availableRerolls.Add(name, result);
                var message = _resultConverter.ToMessage(result, dicePool - hunger);
                await command.RespondAsync(embed: message.Build());
                return;

        }
        await command.RespondAsync($"You executed {command.Data.Name}");
    }

    //private async Task ButtonHandler(SocketSlashCommand command)
    //{
    //    if (command.Data.Name ==)
    //}
}