using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IItemMentionResolver
    {
        bool IsMentionSupported(IItemMention mention);
        
        /// <exception cref="UnsupportedMentionException">If IsMentionSupported returned false</exception>
        Task<IItemReference?> ResolveMentionAsync(IItemMention mention);
    }

    public interface IItemMentionResolver<in TMention> : IItemMentionResolver
        where TMention : IItemMention
    {
        Task<IItemReference?> ResolveMentionAsync(TMention mention);
    }
}