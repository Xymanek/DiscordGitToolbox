using System;
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
            new Regex(@"(?<repo>[A-Za-z0-9_.-]*)#(?<number>\d+)", RegexOptions.Compiled);

        private readonly GitHubMentionConfiguration _mentionConfiguration;
        private readonly IMapper _mapper;
        
        private readonly Lazy<IDictionary<string, GitHubRepositoryReference>> _aliasMap;

        public GitHubMentionExtractor(GitHubMentionConfiguration mentionConfiguration, IMapper mapper)
        {
            _mentionConfiguration = mentionConfiguration;
            _mapper = mapper;
            
            _aliasMap = new Lazy<IDictionary<string, GitHubRepositoryReference>>(MapAliasesToRepositories);
        }

        public IEnumerable<IMention> ExtractMentions(string message)
        {
            if (_aliasMap.Value.Count == 0) yield break;

            MatchCollection matchers = ItemMatcherRegex.Matches(message);
            
            foreach (Match? match in matchers)
            {
                GroupCollection? groups = match?.Groups;
                if (groups?["repo"] == null || groups["number"] == null) continue;
                
                if (
                    _aliasMap.Value.TryGetValue(groups["repo"].Value, out GitHubRepositoryReference repoRef) &&
                    int.TryParse(groups["number"].Value, out int number)
                )
                {
                    yield return new GitHubMention(repoRef, number);
                }
            }
        }

        private IDictionary<string, GitHubRepositoryReference> MapAliasesToRepositories()
        {
            Dictionary<string, GitHubRepositoryReference> result = new Dictionary<string, GitHubRepositoryReference>();
            
            foreach (GitHubMentionConfiguration.Repository repository in _mentionConfiguration.Repositories)
            {
                var repoRef = _mapper.Map<GitHubRepositoryReference>(repository);

                foreach (string alias in repository.Aliases.Append(repoRef.ToString()))
                {
                    result[alias] = repoRef;
                }
            }

            return result;
        }
    }
}