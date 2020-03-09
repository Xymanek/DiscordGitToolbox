using System.Collections.Generic;

namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IMentionExtractor
    {
        IEnumerable<IMention> ExtractMentions(IMentionResolutionContext mentionContext);
    }
}