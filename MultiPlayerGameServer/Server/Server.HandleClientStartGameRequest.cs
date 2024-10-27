using MultiPlayerGame.Game.RoomManager;
using MultiPlayerGame.ServicesInjection;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientStartGameRequest(MessageChannel sender, StartGameRequest request) {
            var requestResult = new RequestResult() {
                ActionCode = ActionCode.GameStart,
                Information = string.Empty,
                IsSuccessful = false,
            };

            int requestRoomID = request.RoomID;
            var roomManager = Services.GetService<RoomManager>();
            var room =  roomManager.FindRoom(requestRoomID);
            if (room == null) {
                requestResult.Information = "房间不存在";
            }
            else {
                requestResult.IsSuccessful = true;
                foreach(var channel in room.PlayerIDToChannelMapping.Values) {
                    channel.SendMessageAsync(request);
                }
            }

            sender.SendMessageAsync(requestResult);
        }
    }
}
