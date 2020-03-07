using System;
using System.Threading.Tasks;
using Octokit;

namespace DiscordGitToolbox.App
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var github = new GitHubClient(new ProductHeaderValue("DiscordGitToolboxApp"));

            PullRequest pr = await github.PullRequest.Get("WOTCStrategyOverhaul", "CovertInfiltration", 458);
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
            }
        }
    }
}