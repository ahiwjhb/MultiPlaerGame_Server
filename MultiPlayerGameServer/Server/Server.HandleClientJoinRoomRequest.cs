using Google.Protobuf.WellKnownTypes;
using MultiPlayerGame.Game.RoomManager;
using MultiPlayerGame.ServicesInjection;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientJoinRoomRequest(MessageChannel sender, JoinRoomRequest request) {
            var requestResult = new RequestResult() {
                ActionCode = ActionCode.JoinRoom,
                IsSuccessful = false,
                Information = string.Empty
            };

            var roomManager = Services.GetService<RoomManager>();

            int joinRoomID = request.JoinRoomID;
            int requesterID = request.RequesterID;

            var room = roomManager.FindRoom(joinRoomID);
            if (room == null) {
                requestResult.Information = "房间已销毁";
            }
            else {
                roomManager.JoinRoom(requesterID, joinRoomID, sender);
                requestResult.IsSuccessful = true;
                requestResult.Information = "加入房间成功";
                requestResult.Args = Any.Pack(roomManager.FindRoom(joinRoomID)!.GetInnerInfo());
                foreach(var channel in room.PlayerIDToChannelMapping.Values) {
                    if(channel != sender) {
                        channel.SendMessageAsync(request);
                    }
                }
            }

            sender.SendMessageAsync(requestResult);
        }
    }
}
