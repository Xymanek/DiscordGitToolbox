using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordGitToolbox.Discord.Commands
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("ping")]
        public Task PingAsync() => ReplyAsync("Pong!");
    }
}