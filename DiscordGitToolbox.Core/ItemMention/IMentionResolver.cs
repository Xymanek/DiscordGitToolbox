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
}