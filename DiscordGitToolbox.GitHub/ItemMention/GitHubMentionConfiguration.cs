using System.Collections.Generic;
using System.Linq;
using Discord;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubMentionConfiguration
    {
        public class Prefix
        {
            public const string FullRepoNamePrefix = "#";
            
            public string Name { get; set; }
            
            public IEnumerable<ulong> ChannelsWhitelist { get; set; }
            public IEnumerable<ulong> ChannelsBlacklist { get; set; }
        }
        
        public class Repository
        {
            public string Owner { get; set; }
            public string Name { get; set; }

            public IEnumerable<Prefix> Prefixes { get; set; }
        }

        public class Guild
        {
            public ulong Id { get; set; }

            public IEnumerable<Repository> Repositories { get; set; }
        }

        public IEnumerable<Guild>? Guilds { get; set; }

        public IEnumerable<Repository>? DefaultRepositories { get; set; }
    }

    public static class GitHubMentionConfigurationExtensions
    {
        public static IEnumerable<GitHubMentionConfiguration.Repository>? GetRepositoriesForGuild
            (this GitHubMentionConfiguration configuration, IGuild guild)
        {
            var configGuild = configuration.Guilds.FirstOrDefault(g => g.Id == guild.Id);

            return configGuild?.Repositories ?? configuration.DefaultRepositories;
        }
    }
}