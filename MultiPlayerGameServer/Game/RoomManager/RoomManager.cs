using Network.MessageChannel;
using Network.Protocol;
using System.Collections.Concurrent;

namespace MultiPlayerGame.Game.RoomManager
{
    public class RoomManager
    {
        private static int AssginedRoomID = 0; 

        private readonly ConcurrentDictionary<int, Room> _idToRoomMapping = new();

        private readonly ConcurrentDictionary<int, int> _playerIDToRoom = new();

        public RoomManager() {
            //CreateRoomRequest request = new CreateRoomRequest() {
            //    RequesterID = 1,
            //    RoomName = "测试房间",
            //    MaxPeopleLimit = 5,
            //};
            //var room = CreateRoom(request);

            //var chatInfo = new ChatInfo() {
            //    SenderID = 1,
            //    SenderName = "测试用户01",
            //    ProfilePhotoBase64 = "",
            //    ChatContent = "测试聊天内容1001011",
            //};
            //room.ChatHistories.Add(chatInfo);
            //chatInfo = new ChatInfo() {
            //    SenderID = 1,
            //    SenderName = "测试用户01",
            //    ProfilePhotoBase64 = "",
            //    ChatContent = "测试聊天内容/nasdasdasdasd/nasdasdasd/n4411001011",
            //};
            //room.ChatHistories.Add(chatInfo);
        }

        public Room CreateRoom(CreateRoomRequest request) {
            AssginedRoomID %= int.MaxValue;
            int id = Interlocked.Increment(ref AssginedRoomID);

            var room = _idToRoomMapping.GetOrAdd(id, id => {
                var room = new Room(id, request.RequesterID);
                room.RoomName = request.RoomName;
                room.MaxPeopleLimit = request.MaxPeopleLimit;
                room.RoomState = RoomState.Watting;
                room.OwnerID = request.RequesterID;
                return room;
            });

            return room;
        }

        public void ExitRoom(int playerID, int exitRoomID) {
            var room = _idToRoomMapping[exitRoomID];
            room.RemovePlayerID(playerID);
            if(room.WattingPeopleNumber == 0) {
                _idToRoomMapping.TryRemove(exitRoomID, out var _);
                _playerIDToRoom.TryRemove(playerID, out var _);
            }
        }

        public IEnumerable<Room> GetRooms() {
            return _idToRoomMapping.Values;
        }

        public Room? FindRoom(int roomID) {
            _idToRoomMapping.TryGetValue(roomID, out var room);
            return room;
        }

        public Room? FindRoomByPlayerID(int palyerID) {
            _idToRoomMapping.TryGetValue(palyerID, out var room);
            return room;
        }

        public bool HasRoom(int roomID) {
            return _idToRoomMapping.TryGetValue(roomID, out var _);
        }

        public void JoinRoom(int playerID, int joinRoomID, MessageChannel messageChannel) {
            var room = _idToRoomMapping[joinRoomID];
            room.AddPlayerID(playerID, messageChannel);
            _idToRoomMapping.GetOrAdd(playerID, _ => room);
        }
    }
}
