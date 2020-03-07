using DiscordGitToolbox.Core;

namespace DiscordGitToolbox.GitHub
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