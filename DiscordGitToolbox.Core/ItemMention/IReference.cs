namespace DiscordGitToolbox.Core.ItemMention
{
    public interface IReference
    {
    }

    public interface ILinkReference : IReference
    {
        string FriendlyUrl { get; }
    }
}