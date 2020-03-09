using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using DiscordGitToolbox.Core.ItemMention;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubMentionExtractor : IMentionExtractor
    {
        private static readonly Regex ItemMatcherRegex =
            new Regex(@"(?<repo>[A-Za-z0-9/_.-]*)#(?<number>\d+)", RegexOptions.Compiled);

        private readonly GitHubMentionConfiguration _mentionConfiguration;
        private readonly IMapper _mapper;

        public GitHubMentionExtractor(GitHubMentionConfiguration mentionConfiguration, IMapper mapper)
        {
            _mentionConfiguration = mentionConfiguration;
            _mapper = mapper;
        }

        public IEnumerable<IMention> ExtractMentions(IMentionResolutionContext mentionContext)
        {
            IDictionary<string, GitHubRepositoryReference> prefixMap = MapPrefixesToRepositories(mentionContext);

            if (prefixMap.Count == 0) yield break;

            MatchCollection matchers = ItemMatcherRegex.Matches(mentionContext.Message.Content);

            foreach (Match? match in matchers)
            {
                GroupCollection? groups = match?.Groups;
                if (groups?["repo"] == null || groups["number"] == null) continue;

                if (
                    prefixMap.TryGetValue(groups["repo"].Value, out GitHubRepositoryReference repoRef) &&
                    int.TryParse(groups["number"].Value, out int number)
                )
                {
                    yield return new GitHubMention(repoRef, number);
                }
            }
        }

        private IDictionary<string, GitHubRepositoryReference> MapPrefixesToRepositories
            (IMentionResolutionContext mentionContext)
        {
            ulong channelId = mentionContext.Message.Channel.Id;

            Dictionary<string, GitHubRepositoryReference> result = new Dictionary<string, GitHubRepositoryReference>();
            IEnumerable<GitHubMentionConfiguration.Repository>? reposConfig =
                _mentionConfiguration.GetRepositoriesForGuild(mentionContext.Guild);

            if (reposConfig == null) return result;

            foreach (GitHubMentionConfiguration.Repository repository in reposConfig)
            {
                var repoRef = _mapper.Map<GitHubRepositoryReference>(repository);

                IEnumerable<string> prefixes = repository.Prefixes
                    .Where(prefix =>
                    {
                        if (prefix.ChannelsWhitelist != null && prefix.ChannelsWhitelist.Contains(channelId))
                        {
                            return true;
                        }

                        if (prefix.ChannelsBlacklist != null && prefix.ChannelsBlacklist.Contains(channelId))
                        {
                            return false;
                        }

                        return true;
                    })
                    .Select(prefix => prefix.Name)
                    .Select(
                        prefix => prefix == GitHubMentionConfiguration.Prefix.FullRepoNamePrefix
                            ? repoRef.ToString()
                            : prefix
                    );

                foreach (string prefix in prefixes)
                {
                    result[prefix] = repoRef;
                }
            }

            return result;
        }
    }
}