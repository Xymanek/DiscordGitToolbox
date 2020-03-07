using DiscordGitToolbox.Core;
using Octokit;

namespace DiscordGitToolbox.GitHub
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