using System.Collections.Generic;
using Octokit;

namespace DiscordGitToolbox.GitHub
{
    public interface IGitHubClientResolver
    {
        GitHubClient GetClientForRepo(GitHubRepositoryReference repo);
    }

    /// <summary>
    /// Facilitates access to private repos
    /// </summary>
    public class GitHubClientCollection : IGitHubClientResolver
    {
        private readonly Dictionary<GitHubRepositoryReference, GitHubClient> _perRepoClients =
            new Dictionary<GitHubRepositoryReference, GitHubClient>();

        private readonly Dictionary<string, GitHubClient> _perOwnerClients =
            new Dictionary<string, GitHubClient>();

        private readonly GitHubClient _unauthenticatedClient;

        public GitHubClientCollection(GitHubClient unauthenticatedClient)
        {
            _unauthenticatedClient = unauthenticatedClient;
        }

        public void SetForRepo(GitHubRepositoryReference repositoryReference, GitHubClient client)
        {
            _perRepoClients[repositoryReference] = client;
        }

        public void SetForOwner(string owner, GitHubClient client)
        {
            _perOwnerClients[owner] = client;
        }

        public GitHubClient GetClientForRepo(GitHubRepositoryReference repo)
        {
            if (_perRepoClients.TryGetValue(repo, out var client)) return client;
            if (_perOwnerClients.TryGetValue(repo.Owner, out client)) return client;

            return _unauthenticatedClient;
        }
    }
}