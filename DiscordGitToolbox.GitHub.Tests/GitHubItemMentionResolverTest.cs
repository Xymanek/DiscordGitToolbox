using System.Threading.Tasks;
using DiscordGitToolbox.GitHub.ItemMention;
using Moq;
using Octokit;
using Xunit;

namespace DiscordGitToolbox.GitHub.Tests
{
    public class GitHubItemMentionResolverTest
    {
        [Fact]
        public async Task TestGetIssueFromMentionAsync_Correct()
        {
            var repo = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var mention = new GitHubItemMention(repo, 123);
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

            var result = await GitHubItemMentionResolver.GetIssueFromMentionAsync(mockClient.Object, mention);
            
            Assert.Same(issue, result);
        }

        [Fact]
        public async Task TestGetIssueFromMentionAsync_InCorrect()
        {
            var repo = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var mention = new GitHubItemMention(repo, 123);

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

            var result = await GitHubItemMentionResolver.GetIssueFromMentionAsync(mockClient.Object, mention);
            
            Assert.Null(result);
        }
    }
}