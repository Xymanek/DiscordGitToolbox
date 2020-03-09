using Discord;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionResolutionContext
    {
        IMessage Message { get; }
        IGuild Guild { get; }
    }

    public class MentionResolutionContext : IMentionResolutionContext
    {
        public MentionResolutionContext(IMessage message, IGuild guild)
        {
            Message = message;
            Guild = guild;
        }

        public IMessage Message { get; }
        public IGuild Guild { get; }
    }
}