using System.Threading.Tasks;
using DiscordGitToolbox.Core.ItemMention;
using Octokit;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubMentionResolver : BaseMentionResolver<GitHubMention>
    {
        private readonly IGitHubClientResolver _clientResolver;

        public GitHubMentionResolver(IGitHubClientResolver clientResolver)
        {
            _clientResolver = clientResolver;
        }

        public override async Task<IReference?> ResolveMentionAsync(GitHubMention mention)
        {
            IGitHubClient client = _clientResolver.GetClientForRepo(mention.RepositoryReference);

            Task<Issue?> issueTask = GetIssueFromMentionAsync(client, mention);
            Task<PullRequest?> prTask = GetPullRequestFromMentionAsync(client, mention);

            await Task.WhenAll(issueTask, prTask);

            Issue? issue = await issueTask;
            PullRequest? pr = await prTask;

            if (issue != null) return new GitHubIssueReference(issue);
            if (pr != null) return new GitHubPullRequestReference(pr);

            return null;
        }

        internal static async Task<Issue?> GetIssueFromMentionAsync(IGitHubClient client, GitHubMention mention)
        {
            try
            {
                return await client.Issue.Get(
                    mention.RepositoryReference.Owner, mention.RepositoryReference.Name, mention.Number
                );
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        internal static async Task<PullRequest?> GetPullRequestFromMentionAsync
            (IGitHubClient client, GitHubMention mention)
        {
            try
            {
                return await client.PullRequest.Get(
                    mention.RepositoryReference.Owner, mention.RepositoryReference.Name, mention.Number
                );
            }
            catch (NotFoundException)
            {
                return null;
            }
        }
    }
}