using System;

namespace DiscordGitToolbox.Core.ItemMention
{
    public class UnsupportedMentionException : Exception
    {
        public readonly IItemMention Mention;

        public UnsupportedMentionException(IItemMention mention)
            : this(mention, $"Mention {mention.GetType()} is not supported")
        {
        }

        public UnsupportedMentionException(IItemMention mention, string? message) : base(message)
        {
            Mention = mention;
        }
    }
}