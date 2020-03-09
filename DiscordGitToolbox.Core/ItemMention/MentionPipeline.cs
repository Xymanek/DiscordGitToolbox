using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionPipeline
    {
        Task<IEnumerable<string>> GetLinksForMessage(IMentionResolutionContext mentionContext);
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

        public async Task<IEnumerable<string>> GetLinksForMessage(IMentionResolutionContext mentionContext)
        {
            string?[] links = await Task.WhenAll(ExtractMentions(mentionContext).Select(ConvertToLinks));

            return links.Where(link => link != null);
        }

        private async Task<string?> ConvertToLinks(IMention mention)
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

        private IEnumerable<IMention> ExtractMentions(IMentionResolutionContext mentionContext)
        {
            return _extractors.SelectMany(extractor => extractor.ExtractMentions(mentionContext));
        }
    }
}