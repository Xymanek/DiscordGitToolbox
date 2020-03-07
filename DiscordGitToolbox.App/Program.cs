using System;
using System.Threading.Tasks;
using AutoMapper;
using DiscordGitToolbox.Core.ItemMention;
using DiscordGitToolbox.GitHub;
using DiscordGitToolbox.GitHub.ItemMention;
using Microsoft.Extensions.DependencyInjection;
using Octokit;

namespace DiscordGitToolbox.App
{
    internal static class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Hello World!");

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider container = serviceCollection.BuildServiceProvider();

            IMentionPipeline pipeline = container.GetRequiredService<IMentionPipeline>();
            
            foreach (string s in await pipeline.GetLinksForMessage("ci#123 chl#456"))
            {
                Console.WriteLine(s);                
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMentionPipeline, MentionPipeline>();
            
            // Github client config
            serviceCollection.AddSingleton(
                provider => new GitHubClientCollection(
                    new GitHubClient(new ProductHeaderValue("DiscordGitToolboxApp"))
                )
            );
            serviceCollection.AddSingleton<IGitHubClientResolver>(
                provider => provider.GetRequiredService<GitHubClientCollection>()
            );
            
            // Github mentions config
            serviceCollection.AddSingleton(provider => new GitHubMentionConfiguration
            {
                Repositories = new []
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

            // Mention resolvers
            serviceCollection.AddSingleton<IMentionResolver, GitHubMentionResolver>();
            
            // Mention extractors
            serviceCollection.AddSingleton<IMentionExtractor, GitHubMentionExtractor>();
            
            // Automapper
            serviceCollection.AddAutoMapper(typeof(GitHubMapperProfile));
        }
    }
}