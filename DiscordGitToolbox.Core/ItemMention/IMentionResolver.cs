using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionResolver
    {
        bool IsMentionSupported(IMention mention);

        /// <exception cref="UnsupportedMentionException">If IsMentionSupported returned false</exception>
        Task<IReference?> ResolveMentionAsync(IMention mention);
    }

    public interface IMentionResolver<in TMention> : IMentionResolver
        where TMention : IMention
    {
        Task<IReference?> ResolveMentionAsync(TMention mention);
    }

    public abstract class BaseMentionResolver<TMention> : IMentionResolver<TMention>
        where TMention : IMention
    {
        public bool IsMentionSupported(IMention mention)
        {
            return mention is TMention;
        }

        public Task<IReference?> ResolveMentionAsync(IMention mention)
        {
            if (!IsMentionSupported(mention))
            {
                throw new UnsupportedMentionException(mention);
            }

            return ResolveMentionAsync((TMention) mention);
        }

        public abstract Task<IReference?> ResolveMentionAsync(TMention mention);
    }
}