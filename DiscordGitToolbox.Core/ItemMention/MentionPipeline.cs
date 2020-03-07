using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionPipeline
    {
        Task<string?> ConvertToMessage(IMention mention);
    }
    
    public class MentionPipeline : IMentionPipeline
    {
        private readonly IEnumerable<IMentionExtractor> _extractors;
        private readonly IEnumerable<IMentionResolver> _resolvers;

        public MentionPipeline(IEnumerable<IMentionExtractor> extractors, IEnumerable<IMentionResolver> resolvers)
        {
            _extractors = extractors;
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

        public IEnumerable<IMention> ExtractMentions(string message)
        {
            return _extractors.SelectMany(extractor => extractor.ExtractMentions(message));
        }
    }
}