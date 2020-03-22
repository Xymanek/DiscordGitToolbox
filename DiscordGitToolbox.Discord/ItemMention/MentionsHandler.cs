using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordGitToolbox.Core.ItemMention;

namespace DiscordGitToolbox.Discord.ItemMention
{
    public class MentionsHandler : ISocketHandler
    {
        private readonly IMentionPipeline _mentionPipeline;
        private readonly IMentionsResponder _responder;

        public MentionsHandler(IMentionPipeline mentionPipeline, IMentionsResponder responder)
        {
            _mentionPipeline = mentionPipeline;
            _responder = responder;
        }

        public void RegisterListeners(BaseSocketClient client)
        {
            client.MessageReceived += MessageReceived;
        }
        
        private Task MessageReceived(SocketMessage message)
        {
            // This can take a while to do as it makes HTTP requests
            // Do not await the gateway context
            Task.Run(() => RespondWithMentionLinks(message));

            return Task.CompletedTask;
        }

        private async Task RespondWithMentionLinks(SocketMessage message)
        {
            SocketGuild guild = ((SocketGuildChannel) message.Channel).Guild;
            var mentionContext = new ResolutionContext(message, guild);

            IResponseContext responseContext = await _mentionPipeline.PrepareResponse(mentionContext);
            await _responder.RespondWithContextAsync(responseContext);
        }
    }
}