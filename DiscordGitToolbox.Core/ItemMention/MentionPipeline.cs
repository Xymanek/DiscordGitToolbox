using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionPipeline
    {
        Task<string?> ConvertToMessage(IItemMention mention);
    }
    
    public class MentionPipeline : IMentionPipeline
    {
        private readonly IEnumerable<IItemMentionResolver> _resolvers;

        public MentionPipeline(IEnumerable<IItemMentionResolver> resolvers)
        {
            _resolvers = resolvers;
        }

        public async Task<string?> ConvertToMessage(IItemMention mention)
        {
            IItemReference? itemReference = null;
            
            foreach (IItemMentionResolver resolver in _resolvers)
            {
                if (!resolver.IsMentionSupported(mention)) continue;
                
                itemReference = await resolver.ResolveMentionAsync(mention);
                if (itemReference != null) break;
            }

            return itemReference?.FriendlyUrl;
        }
    }
}