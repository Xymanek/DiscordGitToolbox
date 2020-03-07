using System;
using System.Threading.Tasks;
using DiscordGitToolbox.Core;
using DiscordGitToolbox.GitHub;
using Octokit;

namespace DiscordGitToolbox.App
{
    internal class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Hello World!");
            
            var github = new GitHubClient(new ProductHeaderValue("DiscordGitToolboxApp"));

            /*PullRequest pr = await github.PullRequest.Get("WOTCStrategyOverhaul", "CovertInfiltration", 458);
            Console.WriteLine("PR: " + pr.Title + " " + pr.HtmlUrl);

            try
            {
                pr = await github.PullRequest.Get("WOTCStrategyOverhaul", "CovertInfiltration", 456);
                Console.WriteLine("PR: " + pr.Title + " " + pr.HtmlUrl);
            }
            catch (NotFoundException)
            {
                Console.WriteLine("PR #456 not found");
                // throw;
            }*/
            
            IItemMentionResolver resolver = new GitHubItemMentionResolver(github);
            var repo = new GitHubRepositoryReference("WOTCStrategyOverhaul", "CovertInfiltration");

            var s458 = await resolver.ResolveMentionAsync(new GitHubItemMention(repo, 458));
            var s456 = await resolver.ResolveMentionAsync(new GitHubItemMention(repo, 456));
            var s900 = await resolver.ResolveMentionAsync(new GitHubItemMention(repo, 900));
            
            Console.WriteLine(s456?.FriendlyUrl);
            Console.WriteLine(s458?.FriendlyUrl);
            Console.WriteLine(s900?.FriendlyUrl);
        }
    }
}