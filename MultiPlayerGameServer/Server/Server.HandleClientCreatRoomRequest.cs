using Google.Protobuf.WellKnownTypes;
using MultiPlayerGame.Game.RoomManager;
using MultiPlayerGame.ServicesInjection;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientCreatRoomRequest(MessageChannel sender, CreateRoomRequest request) {
            var requestResult = new RequestResult() {
                ActionCode = ActionCode.CreateRoom,
                IsSuccessful = false,
                Information = string.Empty
            };

            if(request.MaxPeopleLimit < 1) {
                requestResult.Information = "房间最大人数必须大于1";
            }
            else {
                var roomManager = Services.GetService<RoomManager>();
                var room = roomManager.CreateRoom(request);
                roomManager.JoinRoom(request.RequesterID, room.ID, sender);
                requestResult.IsSuccessful = true;
                requestResult.Information = "创建房间成功";
                requestResult.Args = Any.Pack(room.GetInnerInfo());
            }
            
            sender.SendMessageAsync(requestResult);
        }
    }
}
