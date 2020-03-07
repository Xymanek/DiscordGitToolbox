using System.Threading.Tasks;
using DiscordGitToolbox.Core;
using Octokit;

namespace DiscordGitToolbox.GitHub
{
    public class GitHubItemMentionResolver : BaseItemMentionResolver<GitHubItemMention>
    {
        private readonly IGitHubClient _gitHubClient;

        public GitHubItemMentionResolver(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
            // _gitHubClient = new GitHubClient(new ProductHeaderValue("DiscordGitToolboxApp"));;
        }

        public override async Task<IItemReference?> ResolveMentionAsync(GitHubItemMention mention)
        {
            Task<Issue?> issueTask = GetIssueFromMentionAsync(mention);
            Task<PullRequest?> prTask = GetPullRequestFromMentionAsync(mention);
            
            await Task.WhenAll(issueTask, prTask);

            Issue? issue = await issueTask;
            PullRequest? pr = await prTask;
            
            if (issue != null) return new GitHubIssueReference(issue);
            if (pr != null) return new GitHubPullRequestReference(pr);

            return null;
        }

        internal async Task<Issue?> GetIssueFromMentionAsync(GitHubItemMention mention)
        {
            try
            {
                return await _gitHubClient.Issue.Get(
                    mention.RepositoryReference.Owner, mention.RepositoryReference.Name, mention.Number
                );
            }
            catch (NotFoundException)
            {
                return null;
            }
        }
        
        internal async Task<PullRequest?> GetPullRequestFromMentionAsync(GitHubItemMention mention)
        {
            try
            {
                return await _gitHubClient.PullRequest.Get(
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