using Discord.WebSocket;

namespace DiscordGitToolbox.Discord
{
    public interface ISocketHandler
    {
        void RegisterListeners(BaseSocketClient client);
    }
}