using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordGitToolbox.Core.ItemMention;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordGitToolbox.Discord
{
    public class DiscordClientWorker : BackgroundService
    {
        private readonly ILogger<DiscordClientWorker> _logger;
        private readonly BaseSocketClient _client;
        private readonly IMentionPipeline _mentionPipeline;
        private readonly DiscordConfiguration _discordConfiguration;

        public DiscordClientWorker(
            ILogger<DiscordClientWorker> logger,
            BaseSocketClient client,
            IMentionPipeline mentionPipeline,
            DiscordConfiguration discordConfiguration
        )
        {
            _logger = logger;
            _client = client;
            _mentionPipeline = mentionPipeline;
            _discordConfiguration = discordConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RegisterHandlers();

            await _client.LoginAsync(TokenType.Bot, _discordConfiguration.Token);
            await _client.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                // _logger.LogInformation("DiscordClientWorker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation($"DiscordClientWorker task is stopping");

            await _client.StopAsync();
        }

        public void RegisterHandlers()
        {
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
        }

        private Task Log(LogMessage msg)
        {
            _logger.Log(msg.Severity.ToLogLevel(), msg.Exception, msg.Message);
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }

            // This can take a while to do as it makes HTTP requests
            // Do not await the gateway context
#pragma warning disable 4014
            Task.Run(() => RespondWithMentionLinks(message));
#pragma warning restore 4014
        }

        private async Task RespondWithMentionLinks(SocketMessage message)
        {
            SocketGuild guild = ((SocketGuildChannel) message.Channel).Guild;
            var mentionContext = new MentionResolutionContext(message, guild);
            
            string[] links = (await _mentionPipeline.GetLinksForMessage(mentionContext)).ToArray();
            if (links.Length == 0) return;

            await message.Channel.SendMessageAsync(string.Join('\n', links));
        }
    }
}