using DiscordGitToolbox.Core.ItemMention;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubMention : IMention
    {
        public GitHubRepositoryReference RepositoryReference { get; }

        public int Number { get; }

        public GitHubMention(GitHubRepositoryReference repositoryReference, int number)
        {
            RepositoryReference = repositoryReference;
            Number = number;
        }
    }
}