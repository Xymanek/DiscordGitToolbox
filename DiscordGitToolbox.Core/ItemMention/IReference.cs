namespace DiscordGitToolbox.Core.ItemMention
{
    // TODO: Switch this to a single  interface with default implementations 
    
    public interface IReference
    {
    }

    public interface ILinkReference : IReference
    {
        string FriendlyUrl { get; }
    }
}