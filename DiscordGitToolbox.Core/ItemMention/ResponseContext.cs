using System.Collections.Generic;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IResponseContext
    {
        /// <summary>
        /// The context that spawned this responseContext
        /// </summary>
        IResolutionContext ResolutionContext { get; }
        
        public IReadOnlyCollection<string> Links { get; }
        
        // In future we may support embeds and other things
    }

    public sealed class FinalizedResponseContext : IResponseContext
    {
        public IResolutionContext ResolutionContext { get; }
        
        public IReadOnlyCollection<string> Links { get; }

        public FinalizedResponseContext(
            IResolutionContext resolutionContext,
            IReadOnlyCollection<string> links
        )
        {
            ResolutionContext = resolutionContext;
            Links = links;
        }
    }
}