using AutoMapper;
using DiscordGitToolbox.GitHub.ItemMention;

namespace DiscordGitToolbox.GitHub
{
    public class GitHubMapperProfile : Profile
    {
        public GitHubMapperProfile()
        {
            CreateMap<GitHubMentionConfiguration.Repository, GitHubRepositoryReference>();
        }
    }
}