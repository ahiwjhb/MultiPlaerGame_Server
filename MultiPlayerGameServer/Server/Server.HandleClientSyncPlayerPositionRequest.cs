using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        public void HandleClientSyncPlayerPositionRequest(MessageChannel sender, SyncPlayerPositionRequest request) {
            foreach(var channel in _clientChannels) {
                if(channel != sender) {
                    channel.SendMessageAsync(request);
                }
            }
        }
    }
}
