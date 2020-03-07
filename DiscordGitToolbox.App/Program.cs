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

            var repo = new GitHubRepositoryReference("WOTCStrategyOverhaul", "CovertInfiltration");
            Console.WriteLine(await pipeline.ConvertToMessage(new GitHubMention(repo, 456)));
            Console.WriteLine(await pipeline.ConvertToMessage(new GitHubMention(repo, 458)));
            Console.WriteLine(await pipeline.ConvertToMessage(new GitHubMention(repo, 900)));
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMentionPipeline, MentionPipeline>();
            
            // Github config
            serviceCollection.AddSingleton(
                provider => new GitHubClientCollection(
                    new GitHubClient(new ProductHeaderValue("DiscordGitToolboxApp"))
                )
            );
            serviceCollection.AddSingleton<IGitHubClientResolver>(
                provider => provider.GetRequiredService<GitHubClientCollection>()
            );

            // Mention resolvers
            serviceCollection.AddSingleton<IMentionResolver, GitHubMentionResolver>();
            
            // Mention extractors
            serviceCollection.AddSingleton<IMentionExtractor, GitHubMentionExtractor>();
            
            // Automapper
            serviceCollection.AddAutoMapper(typeof(GitHubMapperProfile));
        }
    }
}