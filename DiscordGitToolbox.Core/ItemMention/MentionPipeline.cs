using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionPipeline
    {
        Task<IResponseContext> PrepareResponse(IResolutionContext context);
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

        public async Task<IResponseContext> PrepareResponse(IResolutionContext context)
        {
            IReference?[] references = await Task.WhenAll(ExtractMentions(context).Select(ConvertToReference));
            IReference[] referencesNotNull = references.Where(r => r != null).ToArray();  

            ReadOnlyCollection<string> links = referencesNotNull
                .Select(r => r.FriendlyUrl)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList()
                .AsReadOnly();
            
            return new FinalizedResponseContext(context, links);
        }

        private async Task<IReference?> ConvertToReference(IMention mention)
        {
            foreach (IMentionResolver resolver in _resolvers)
            {
                if (!resolver.IsMentionSupported(mention)) continue;
                
                IReference? itemReference = await resolver.ResolveMentionAsync(mention);
                if (itemReference != null) return itemReference;
            }

            return null;
        }

        private IEnumerable<IMention> ExtractMentions(IResolutionContext context)
        {
            return _extractors.SelectMany(extractor => extractor.ExtractMentions(context));
        }
    }
}