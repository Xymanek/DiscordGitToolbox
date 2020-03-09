using System.Threading.Tasks;
using AutoMapper;
using Discord.WebSocket;
using DiscordGitToolbox.Core.ItemMention;
using DiscordGitToolbox.Discord;
using DiscordGitToolbox.GitHub;
using DiscordGitToolbox.GitHub.ItemMention;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octokit;

namespace DiscordGitToolbox.App
{
    public static class Program
    {
        private static ILogger _globalLogger;
        
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            _globalLogger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("AppGlobal");
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            
            host.Run();
        }

        private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e.Observed) return;
            
            _globalLogger.LogWarning(e.Exception, "UnobservedTaskException");
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureCoreServices)
                .ConfigureServices(ConfigureDiscordServices)
                .ConfigureServices(ConfigureGitHubServices)
                .ConfigureServices(ConfigureSharedServices);

        private static void ConfigureCoreServices(IServiceCollection services)
        {
            // Mentions pipeline
            services.AddSingleton<IMentionPipeline, MentionPipeline>();
        }

        private static void ConfigureDiscordServices(IServiceCollection services)
        {
            services.AddSingleton(
                provider => provider
                    .GetRequiredService<IConfiguration>()
                    .GetSection("Discord")
                    .Get<DiscordConfiguration>() ?? new DiscordConfiguration() // If we fail (no env/secret set), do not nullref
            );

            // Discord client
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<BaseSocketClient>(
                provider => provider.GetRequiredService<DiscordSocketClient>()
            );

            // Discord worker
            services.AddHostedService<DiscordClientWorker>();
        }

        private static void ConfigureGitHubServices(IServiceCollection services)
        {
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

            services.AddSingleton<IMentionResolver, GitHubMentionResolver>();
            services.AddSingleton<IMentionExtractor, GitHubMentionExtractor>();
        }

        private static void ConfigureSharedServices(IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(GitHubMapperProfile));
        }
    }
}