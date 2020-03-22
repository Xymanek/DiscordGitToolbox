using DiscordGitToolbox.Core.ItemMention;
using Octokit;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubIssueReference : ILinkReference
    {
        public Issue Issue { get; }

        public GitHubIssueReference(Issue issue)
        {
            Issue = issue;
        }

        public string FriendlyUrl => Issue.HtmlUrl;
    }

    public class GitHubPullRequestReference : ILinkReference
    {
        public PullRequest PullRequest { get; }

        public GitHubPullRequestReference(PullRequest pullRequest)
        {
            PullRequest = pullRequest;
        }

        public string FriendlyUrl => PullRequest.HtmlUrl;
    }
}