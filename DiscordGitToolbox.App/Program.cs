using AutoMapper;
using Discord.WebSocket;
using DiscordGitToolbox.Core.ItemMention;
using DiscordGitToolbox.GitHub;
using DiscordGitToolbox.GitHub.ItemMention;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;

namespace DiscordGitToolbox.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => { ConfigureServices(services); });
        
        private static void ConfigureServices(IServiceCollection services)
        {
            // Discord client
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<BaseSocketClient>(
                provider => provider.GetRequiredService<DiscordSocketClient>()
            );

            // Discord worker
            services.AddHostedService<DiscordClientWorker>();

            // Github client config
            services.AddSingleton(
                provider => new GitHubClientCollection(
                    new GitHubClient(new ProductHeaderValue("DiscordGitToolboxApp"))
                )
            );
            services.AddSingleton<IGitHubClientResolver>(
                provider => provider.GetRequiredService<GitHubClientCollection>()
            );

            // Github mentions config
            services.AddSingleton(provider => new GitHubMentionConfiguration
            {
                Repositories = new[]
                {
                    new GitHubMentionConfiguration.Repository
                    {
                        Owner = "WOTCStrategyOverhaul",
                        Name = "CovertInfiltration",

                        Aliases = new[] {"", "ci", "CI"}
                    },
                    new GitHubMentionConfiguration.Repository
                    {
                        Owner = "X2CommunityCore",
                        Name = "X2WOTCCommunityHighlander",

                        Aliases = new[] {"chl", "CHL"}
                    }
                }
            });
            services.AddSingleton(
                provider => provider
                    .GetRequiredService<IConfiguration>()
                    .GetSection("Discord")
                    .Get<DiscordConfiguration>()
            );

            // Mentions pipeline
            services.AddSingleton<IMentionPipeline, MentionPipeline>();

            // Mention resolvers
            services.AddSingleton<IMentionResolver, GitHubMentionResolver>();

            // Mention extractors
            services.AddSingleton<IMentionExtractor, GitHubMentionExtractor>();

            // Automapper
            services.AddAutoMapper(typeof(GitHubMapperProfile));
        }
    }
}