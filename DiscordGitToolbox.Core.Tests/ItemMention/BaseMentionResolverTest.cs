using System.Threading.Tasks;
using DiscordGitToolbox.Core.ItemMention;
using Moq;
using Xunit;

namespace DiscordGitToolbox.Core.Tests.ItemMention
{
    public class BaseMentionResolverTest
    {
        [Fact]
        public async Task TestUnsupportedMentionException()
        {
            var invalidMention = new Mock<IMention>().Object;
            var resolver = new EmptyMentionResolver();

            UnsupportedMentionException exception = await Assert.ThrowsAsync<UnsupportedMentionException>(
                () => resolver.ResolveMentionAsync(invalidMention)
            );
            Assert.Same(invalidMention, exception.Mention);
        }

        [Fact]
        public void TestIsSupported()
        {
            var resolver = new EmptyMentionResolver();

            var validMention = new EmptyMention();
            var invalidMention = new Mock<IMention>().Object;

            Assert.True(resolver.IsMentionSupported(validMention));
            Assert.False(resolver.IsMentionSupported(invalidMention));

            Assert.True(false, "Test breaking github actions 4"); // Test breaking github actions
        }

        private class EmptyMention : IMention
        {
        }

        private class EmptyMentionResolver : BaseMentionResolver<EmptyMention>
        {
            public override Task<IReference?> ResolveMentionAsync(EmptyMention mention)
            {
                return Task.FromResult<IReference?>(null);
            }
        }
    }
}