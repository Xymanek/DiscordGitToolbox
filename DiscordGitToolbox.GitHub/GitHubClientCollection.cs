using System.Collections.Generic;
using Octokit;

namespace DiscordGitToolbox.GitHub
{
    public interface IGitHubClientResolver
    {
        IGitHubClient GetClientForRepo(GitHubRepositoryReference repo);
    }

    /// <summary>
    /// Facilitates access to private repos
    /// </summary>
    public class GitHubClientCollection : IGitHubClientResolver
    {
        private readonly Dictionary<GitHubRepositoryReference, IGitHubClient> _perRepoClients =
            new Dictionary<GitHubRepositoryReference, IGitHubClient>();

        private readonly Dictionary<string, IGitHubClient> _perOwnerClients =
            new Dictionary<string, IGitHubClient>();

        private readonly IGitHubClient _unauthenticatedClient;

        public GitHubClientCollection(IGitHubClient unauthenticatedClient)
        {
            _unauthenticatedClient = unauthenticatedClient;
        }

        public void SetForRepo(GitHubRepositoryReference repositoryReference, IGitHubClient client)
        {
            _perRepoClients[repositoryReference] = client;
        }

        public void SetForOwner(string owner, IGitHubClient client)
        {
            _perOwnerClients[owner] = client;
        }

        public IGitHubClient GetClientForRepo(GitHubRepositoryReference repo)
        {
            if (_perRepoClients.TryGetValue(repo, out var client)) return client;
            if (_perOwnerClients.TryGetValue(repo.Owner, out client)) return client;

            return _unauthenticatedClient;
        }
    }
}