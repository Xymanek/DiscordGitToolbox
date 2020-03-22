using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordGitToolbox.Discord
{
    public interface ISocketHandler
    {
        /// <remarks>
        /// This will be called on all handlers in parallel (using Task.WhenAll).
        /// Try to use this to speed up startup.
        /// <br/>
        /// Called before <see cref="InitializeAsync"/> 
        /// </remarks>
        public Task InitializeAsyncBatch()
        {
            return Task.CompletedTask;
        }

        /// <remarks>
        /// Use this if you need to mutate the discord client<br/>
        /// Called before <see cref="RegisterListeners"/> 
        /// </remarks>
        public Task InitializeAsync(BaseSocketClient client)
        {
            return Task.CompletedTask;
        }

        void RegisterListeners(BaseSocketClient client);
    }
}