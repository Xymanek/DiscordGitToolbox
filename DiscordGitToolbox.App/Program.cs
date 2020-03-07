using System;
using System.Threading.Tasks;
using AutoMapper;
using Discord;
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
        private static async Task Main()
        {
            Console.WriteLine("Hello World!");

            await Discord();

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider container = serviceCollection.BuildServiceProvider();

            IMentionPipeline pipeline = container.GetRequiredService<IMentionPipeline>();
            
            foreach (string s in await pipeline.GetLinksForMessage("ci#123 chl#456"))
            {
                Console.WriteLine(s);                
            }
        }
        
        private static DiscordSocketClient _client;

        private static async Task Discord()
        {
            _client = new DiscordSocketClient();
            
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        private static async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
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
        
        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}