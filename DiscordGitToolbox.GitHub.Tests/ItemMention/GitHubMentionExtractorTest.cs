using System.Collections.Generic;
using AutoMapper;
using DiscordGitToolbox.Core.ItemMention;
using DiscordGitToolbox.GitHub.ItemMention;
using Moq;
using Xunit;

namespace DiscordGitToolbox.GitHub.Tests.ItemMention
{
    public class GitHubMentionExtractorTest
    {
        /*[Fact]
        public void TestExtractMentions()
        {
            var repoRef = new GitHubRepositoryReference("AwesomeOrg", "AwesomeRepo");
            var configRepo = new GitHubMentionConfiguration.Repository
            {
                Owner = repoRef.Owner,
                Name = repoRef.Name,

                Prefixes = new[] {"", "ar", "AR"}
            };

            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(mapper => mapper.Map<GitHubRepositoryReference>(
                    It.Is<GitHubMentionConfiguration.Repository>(x => x == configRepo)
                ))
                .Returns(repoRef);

            var extractor = new GitHubMentionExtractor(
                new GitHubMentionConfiguration {DefaultRepositories = new[] {configRepo}},
                mockMapper.Object
            );

            const string str = "asd sd#qwe sdq#123 #12 .asd.,asd.asd#324vawes vgthswaWER ar# ar ar#45 AR#78";

            IEnumerable<IMention> result = extractor.ExtractMentions(str);
            
            Assert.Collection(
                result,
                mention =>
                {
                    var githubMention = (GitHubMention) mention;
                    
                    Assert.Equal(repoRef, githubMention.RepositoryReference);
                    Assert.Equal(12, githubMention.Number);
                },
                mention =>
                {
                    var githubMention = (GitHubMention) mention;
                    
                    Assert.Equal(repoRef, githubMention.RepositoryReference);
                    Assert.Equal(45, githubMention.Number);
                },
                mention =>
                {
                    var githubMention = (GitHubMention) mention;
                    
                    Assert.Equal(repoRef, githubMention.RepositoryReference);
                    Assert.Equal(78, githubMention.Number);
                }
            );
        }*/
    }
}