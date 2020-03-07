using System.Threading.Tasks;

namespace DiscordGitToolbox.Core
{
    public abstract class BaseItemMentionResolver<TMention> : IItemMentionResolver<TMention>
        where TMention : IItemMention
    {
        public bool IsMentionSupported(IItemMention mention)
        {
            return mention is TMention;
        }

        public async Task<IItemReference> ResolveMentionAsync(IItemMention mention)
        {
            if (!IsMentionSupported(mention))
            {
                throw new UnsupportedMentionException(mention);
            }

            return await ResolveMentionAsync((TMention) mention);
        }

        public abstract Task<IItemReference?> ResolveMentionAsync(TMention mention);
    }
}