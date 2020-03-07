using System;

namespace DiscordGitToolbox.Core.ItemMention
{
    public class UnsupportedMentionException : Exception
    {
        public readonly IMention Mention;

        public UnsupportedMentionException(IMention mention)
            : this(mention, $"Mention {mention.GetType()} is not supported")
        {
        }

        public UnsupportedMentionException(IMention mention, string? message) : base(message)
        {
            Mention = mention;
        }
    }
}