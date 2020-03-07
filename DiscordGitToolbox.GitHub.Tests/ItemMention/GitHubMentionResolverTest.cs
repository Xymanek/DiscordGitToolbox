using System.Threading.Tasks;
using DiscordGitToolbox.GitHub.ItemMention;
using Moq;
using Octokit;
using Xunit;

namespace DiscordGitToolbox.GitHub.Tests.ItemMention
{
    public class GitHubMentionResolverTest
    {
        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task TestResolveMentionAsync(bool isIssue, bool isPr)
        {
            var repo = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var mention = new GitHubMention(repo, 123);

            var issue = new Issue();
            var pr = new PullRequest();

            var mockIssues = new Mock<IIssuesClient>();
            var issueGetSetup = mockIssues.Setup(i => i.Get(
                It.Is<string>(s => s == repo.Owner),
                It.Is<string>(s => s == repo.Name),
                It.Is<int>(i => i == mention.Number)
            ));

            if (isIssue) issueGetSetup.Returns(Task.FromResult(issue));
            else issueGetSetup.ThrowsAsync(new NotFoundException(new Mock<IResponse>().Object));

            var mockPrs = new Mock<IPullRequestsClient>();
            var prGetSetup = mockPrs.Setup(i => i.Get(
                It.Is<string>(s => s == repo.Owner),
                It.Is<string>(s => s == repo.Name),
                It.Is<int>(i => i == mention.Number)
            ));

            if (isPr) prGetSetup.Returns(Task.FromResult(pr));
            else prGetSetup.ThrowsAsync(new NotFoundException(new Mock<IResponse>().Object));

            var mockClient = new Mock<IGitHubClient>();
            mockClient.Setup(c => c.Issue).Returns(mockIssues.Object);
            mockClient.Setup(c => c.PullRequest).Returns(mockPrs.Object);
            
            var mockClientResolver = new Mock<IGitHubClientResolver>();
            mockClientResolver
                .Setup(c => c.GetClientForRepo(It.Is<GitHubRepositoryReference>(r => r == repo)))
                .Returns(mockClient.Object);
                
            var result = await new GitHubMentionResolver(mockClientResolver.Object)
                .ResolveMentionAsync(mention);

            if (isIssue) Assert.IsType<GitHubIssueReference>(result);
            else if (isPr) Assert.IsType<GitHubPullRequestReference>(result);
            else Assert.Null(result);
        }

        [Fact]
        public async Task TestGetIssueFromMentionAsync_Correct()
        {
            var repo = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var mention = new GitHubMention(repo, 123);
            var issue = new Issue();

            var mockIssues = new Mock<IIssuesClient>();
            mockIssues
                .Setup(i => i.Get(
                    It.Is<string>(s => s == repo.Owner),
                    It.Is<string>(s => s == repo.Name),
                    It.Is<int>(i => i == mention.Number)
                ))
                .Returns(Task.FromResult(issue));

            var mockClient = new Mock<IGitHubClient>();
            mockClient.Setup(c => c.Issue).Returns(mockIssues.Object);

            var result = await GitHubMentionResolver.GetIssueFromMentionAsync(mockClient.Object, mention);

            Assert.Same(issue, result);
        }

        [Fact]
        public async Task TestGetIssueFromMentionAsync_InCorrect()
        {
            var repo = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var mention = new GitHubMention(repo, 123);

            var mockIssues = new Mock<IIssuesClient>();
            mockIssues
                .Setup(i => i.Get(
                    It.Is<string>(s => s == repo.Owner),
                    It.Is<string>(s => s == repo.Name),
                    It.Is<int>(i => i == mention.Number)
                ))
                .ThrowsAsync(new NotFoundException(new Mock<IResponse>().Object));

            var mockClient = new Mock<IGitHubClient>();
            mockClient.Setup(c => c.Issue).Returns(mockIssues.Object);

            var result = await GitHubMentionResolver.GetIssueFromMentionAsync(mockClient.Object, mention);

            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetPullRequestFromMentionAsync_Correct()
        {
            var repo = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var mention = new GitHubMention(repo, 123);
            var pr = new PullRequest();

            var mockPrs = new Mock<IPullRequestsClient>();
            mockPrs
                .Setup(i => i.Get(
                    It.Is<string>(s => s == repo.Owner),
                    It.Is<string>(s => s == repo.Name),
                    It.Is<int>(i => i == mention.Number)
                ))
                .Returns(Task.FromResult(pr));

            var mockClient = new Mock<IGitHubClient>();
            mockClient.Setup(c => c.PullRequest).Returns(mockPrs.Object);

            var result = await GitHubMentionResolver.GetPullRequestFromMentionAsync(mockClient.Object, mention);

            Assert.Same(pr, result);
        }

        [Fact]
        public async Task TestGetPullRequestFromMentionAsync_InCorrect()
        {
            var repo = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var mention = new GitHubMention(repo, 123);

            var mockPrs = new Mock<IPullRequestsClient>();
            mockPrs
                .Setup(i => i.Get(
                    It.Is<string>(s => s == repo.Owner),
                    It.Is<string>(s => s == repo.Name),
                    It.Is<int>(i => i == mention.Number)
                ))
                .ThrowsAsync(new NotFoundException(new Mock<IResponse>().Object));

            var mockClient = new Mock<IGitHubClient>();
            mockClient.Setup(c => c.PullRequest).Returns(mockPrs.Object);

            var result = await GitHubMentionResolver.GetPullRequestFromMentionAsync(mockClient.Object, mention);

            Assert.Null(result);
        }
    }
}