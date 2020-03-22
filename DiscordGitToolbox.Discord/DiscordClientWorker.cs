using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordGitToolbox.Discord
{
    public class DiscordClientWorker : BackgroundService
    {
        private readonly ILogger<DiscordClientWorker> _logger;
        private readonly BaseSocketClient _client;
        private readonly DiscordConfiguration _discordConfiguration;
        private readonly IEnumerable<ISocketHandler> _handlers;

        public DiscordClientWorker(
            ILogger<DiscordClientWorker> logger,
            BaseSocketClient client,
            DiscordConfiguration discordConfiguration,
            IEnumerable<ISocketHandler> handlers
        )
        {
            _logger = logger;
            _client = client;
            _discordConfiguration = discordConfiguration;
            _handlers = handlers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeHandlers();
            
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

        private async Task InitializeHandlers()
        {
            await Task.WhenAll(_handlers.Select(handler => handler.InitializeAsyncBatch()));

            foreach (ISocketHandler handler in _handlers)
            {
                await handler.InitializeAsync(_client);
            }
        }

        private void RegisterHandlers()
        {
            _client.Log += Log;

            foreach (ISocketHandler handler in _handlers)
            {
                handler.RegisterListeners(_client);
            }
        }

        private Task Log(LogMessage msg)
        {
            _logger.Log(msg.Severity.ToLogLevel(), msg.Exception, msg.Message);
            return Task.CompletedTask;
        }
    }
}