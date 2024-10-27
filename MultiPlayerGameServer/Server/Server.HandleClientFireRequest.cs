using MultiPlayerGame.Game.RoomManager;
using MultiPlayerGame.ServicesInjection;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientFireRequest(MessageChannel sender, FireRequest request) {
            var roomManager = Services.GetService<RoomManager>();
            foreach (var channel in roomManager.FindRoomByPlayerID(request.RequesterID)!.PlayerIDToChannelMapping.Values) {
                channel.SendMessageAsync(request);
            }
        }
    }
}
