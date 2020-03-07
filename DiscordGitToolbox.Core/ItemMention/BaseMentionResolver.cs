using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public abstract class BaseMentionResolver<TMention> : IMentionResolver<TMention>
        where TMention : IMention
    {
        public bool IsMentionSupported(IMention mention)
        {
            return mention is TMention;
        }

        public async Task<IReference?> ResolveMentionAsync(IMention mention)
        {
            if (!IsMentionSupported(mention))
            {
                throw new UnsupportedMentionException(mention);
            }

            return await ResolveMentionAsync((TMention) mention);
        }

        public abstract Task<IReference?> ResolveMentionAsync(TMention mention);
    }
}