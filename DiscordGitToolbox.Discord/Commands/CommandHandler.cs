using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordGitToolbox.Discord.Commands
{
    public class CommandHandler : ISocketHandler
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;

        public CommandHandler(
            IServiceProvider services,
            CommandService commands,
            DiscordSocketClient client // I wish I could inject the abstract class here, but SocketCommandContext requires a concrete one
        )
        {
            _commands = commands;
            _client = client;
            _services = services;
        }

        public async Task InitializeAsyncBatch()
        {
            await _commands.AddModulesAsync(typeof(CommandHandler).Assembly, _services);
        }

        public void RegisterListeners(BaseSocketClient client)
        {
            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            // Don't process the command if it was a system message
            if (!(msg is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (
                !(
                    message.HasCharPrefix('!', ref argPos) || // TODO: Config
                    message.HasMentionPrefix(_client.CurrentUser, ref argPos)
                ) ||
                message.Author.IsBot
            )
            {
                return;
            }

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            IResult result = await _commands.ExecuteAsync(context, argPos, _services);

            // Optionally, we may inform the user if the command fails
            // to be executed; however, this may not always be desired,
            // as it may clog up the request queue should a user spam a
            // command.
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}