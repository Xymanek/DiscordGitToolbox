using System.Threading.Tasks;
using DiscordGitToolbox.Core.ItemMention;

namespace DiscordGitToolbox.Discord.ItemMention
{
    public interface IMentionsResponder
    {
        Task RespondWithContextAsync(IResponseContext context);
    }
    
    public class MentionsResponder : IMentionsResponder
    {
        public async Task RespondWithContextAsync(IResponseContext context)
        {
            if (context.Links.Count > 0)
            {
                await context.ResolutionContext.Message.Channel.SendMessageAsync(string.Join('\n', context.Links));
            }
            
            // In future we may have embeds, etc
        }
    }
}