using System;
using System.Threading.Tasks;
using AutoMapper;
using Discord.WebSocket;
using DiscordGitToolbox.Core.ItemMention;
using DiscordGitToolbox.GitHub;
using DiscordGitToolbox.GitHub.ItemMention;
using Microsoft.Extensions.DependencyInjection;
using Octokit;

namespace DiscordGitToolbox.App
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Hello World!");

            // await Discord();

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider container = serviceCollection.BuildServiceProvider();

            var discordHandler = container.GetRequiredService<DiscordEventsHandler>();

            discordHandler.RegisterHandlers();

            Task worker = Task.Factory.StartNew(() => discordHandler.ConnectAndWork(), TaskCreationOptions.LongRunning);
            // worker.Wait();

            Console.ReadLine();

            /*IMentionPipeline pipeline = container.GetRequiredService<IMentionPipeline>();
            
            foreach (string s in await pipeline.GetLinksForMessage("ci#123 chl#456"))
            {
                Console.WriteLine(s);                
            }*/
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Discord client
            serviceCollection.AddSingleton<DiscordSocketClient>();
            serviceCollection.AddSingleton<BaseSocketClient>(
                provider => provider.GetRequiredService<DiscordSocketClient>()
            );

            // Discord handler
            serviceCollection.AddSingleton<DiscordEventsHandler>();

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

            // Mentions pipeline
            serviceCollection.AddSingleton<IMentionPipeline, MentionPipeline>();

            // Mention resolvers
            serviceCollection.AddSingleton<IMentionResolver, GitHubMentionResolver>();

            // Mention extractors
            serviceCollection.AddSingleton<IMentionExtractor, GitHubMentionExtractor>();

            // Automapper
            serviceCollection.AddAutoMapper(typeof(GitHubMapperProfile));
        }
    }
}