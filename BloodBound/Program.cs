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
        _token = Environment.GetEnvironmentVariable("DISCORDTOKEN");
        _rollService = new Diceroller();
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
        globalRollPoolCommand.AddOption("pool", ApplicationCommandOptionType.Integer,"amount in the dicepool", isRequired: true);
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

            case "roll pool":
                return;

        }
        await command.RespondAsync($"You executed {command.Data.Name}");
        
    }
}