using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordGitToolbox.Core.ItemMention;

namespace DiscordGitToolbox.App
{
    public class DiscordEventsHandler
    {
        private readonly BaseSocketClient _client;
        private readonly  IMentionPipeline _mentionPipeline;

        public DiscordEventsHandler(BaseSocketClient client, IMentionPipeline mentionPipeline)
        {
            _client = client;
            _mentionPipeline = mentionPipeline;
        }
        
        public void RegisterHandlers()
        {
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
        }

        public async Task ConnectAndWork()
        {
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
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
            string[] links = (await _mentionPipeline.GetLinksForMessage(message.Content)).ToArray();
            if (links.Length == 0) return;
            
            await message.Channel.SendMessageAsync(string.Join('\n', links));
        }
    }
}