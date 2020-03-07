using System.Collections.Generic;

namespace DiscordGitToolbox.GitHub.ItemMention
{
    public class GitHubMentionConfiguration
    {
        public class Repository
        {
            public string Owner { get; set; }
            public string Name { get; set; }

            public IEnumerable<string> Aliases { get; set; }
        }
        
        public IEnumerable<Repository> Repositories { get; set; }
    }
}