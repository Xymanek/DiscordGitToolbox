using Discord;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IResolutionContext
    {
        IMessage Message { get; }
        
        IGuild Guild { get; }
    }

    public class ResolutionContext : IResolutionContext
    {
        public ResolutionContext(IMessage message, IGuild guild)
        {
            Message = message;
            Guild = guild;
        }

        public IMessage Message { get; }
        
        public IGuild Guild { get; }
    }
}