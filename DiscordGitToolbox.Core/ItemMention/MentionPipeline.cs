using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionPipeline
    {
        Task<string?> ConvertToMessage(IMention mention);
    }
    
    public class MentionPipeline : IMentionPipeline
    {
        private readonly IEnumerable<IMentionResolver> _resolvers;

        public MentionPipeline(IEnumerable<IMentionResolver> resolvers)
        {
            _resolvers = resolvers;
        }

        public async Task<string?> ConvertToMessage(IMention mention)
        {
            IReference? itemReference = null;
            
            foreach (IMentionResolver resolver in _resolvers)
            {
                if (!resolver.IsMentionSupported(mention)) continue;
                
                itemReference = await resolver.ResolveMentionAsync(mention);
                if (itemReference != null) break;
            }

            return itemReference?.FriendlyUrl;
        }
    }
}