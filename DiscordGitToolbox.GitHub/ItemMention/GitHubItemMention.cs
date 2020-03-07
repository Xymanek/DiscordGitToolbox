using DiscordGitToolbox.Core.ItemMention;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubItemMention : IItemMention
    {
        public GitHubRepositoryReference RepositoryReference { get; }

        public int Number { get; }

        public GitHubItemMention(GitHubRepositoryReference repositoryReference, int number)
        {
            RepositoryReference = repositoryReference;
            Number = number;
        }
    }
}