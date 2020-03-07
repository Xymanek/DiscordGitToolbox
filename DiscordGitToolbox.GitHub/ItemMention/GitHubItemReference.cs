using DiscordGitToolbox.Core.ItemMention;
using Octokit;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubIssueReference : IItemReference
    {
        public Issue Issue { get; }

        public GitHubIssueReference(Issue issue)
        {
            Issue = issue;
        }

        public string FriendlyUrl => Issue.HtmlUrl;
    }

    public class GitHubPullRequestReference : IItemReference
    {
        public PullRequest PullRequest { get; }

        public GitHubPullRequestReference(PullRequest pullRequest)
        {
            PullRequest = pullRequest;
        }

        public string FriendlyUrl => PullRequest.HtmlUrl;
    }
}