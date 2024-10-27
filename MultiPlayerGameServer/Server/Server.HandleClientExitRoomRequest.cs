using MultiPlayerGame.Game.RoomManager;
using MultiPlayerGame.ServicesInjection;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientExitRoomRequest(MessageChannel sender, ExitRoomRequest request) {
            var requestResult = new RequestResult() {
                ActionCode = ActionCode.ExitRoom,
                IsSuccessful = false,
                Information = string.Empty
            };

            var roomManager = Services.GetService<RoomManager>();
            var room = roomManager.FindRoom(request.ExitRoomID);
            if (room != null) {
                roomManager.ExitRoom(request.RequesterID, request.ExitRoomID);
                requestResult.IsSuccessful = true;
                foreach (var channel in room.PlayerIDToChannelMapping.Values) {
                    if(channel != sender) {
                        channel.SendMessageAsync(request);
                    }
                }
            }

            sender.SendMessageAsync(requestResult);
        }
    }
}
