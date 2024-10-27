using Google.Protobuf.WellKnownTypes;
using MultiPlayerGame.Game.RoomManager;
using MultiPlayerGame.ServicesInjection;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientSearchRoomRequest(MessageChannel sender, SearchRoomRequest request) {
            var searchResult = new RequestResult() {
                ActionCode = ActionCode.SearchRoom,
                IsSuccessful = false,
                Information = string.Empty,
            };

            var roomManager = Services.GetService<RoomManager>();
            var roomListPack = new RoomListPack();
            string searchName = request.RoomName;
            if (string.IsNullOrEmpty(searchName)) {
                searchResult.Information = "房间名不能为空";
            }
            else {
                foreach(var room in roomManager.GetRooms().Where(room => searchName == "*" || room.RoomName.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) != -1)) {
                    roomListPack.RoomList.Add(room.GetDisplayInfo());
                }
                searchResult.IsSuccessful = true;
                searchResult.Information = "查询成功";
                searchResult.Args = Any.Pack(roomListPack);
            }

            sender.SendMessageAsync(searchResult);
        }
    }
}
