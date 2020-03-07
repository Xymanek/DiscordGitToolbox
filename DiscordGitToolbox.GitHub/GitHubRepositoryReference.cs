namespace DiscordGitToolbox.GitHub
{
    public class GitHubRepositoryReference
    {
        public readonly string Owner;
        public readonly string Name;

        public GitHubRepositoryReference(string owner, string name)
        {
            Owner = owner;
            Name = name;
        }
    }
}