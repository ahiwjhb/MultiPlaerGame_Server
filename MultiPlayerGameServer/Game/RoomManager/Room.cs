using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Game.RoomManager
{   
    public class Room
    {
        private readonly RoomDisplayInfo _displayInfo = new();

        private readonly RoomInnerInfo _innerInfo = new();

        public Dictionary<int, MessageChannel> PlayerIDToChannelMapping { get; } = new();

        public int ID {
            get => _displayInfo.ID;
            private set => _displayInfo.ID = _innerInfo.ID = value;
        }

        public int OwnerID {
            get => _displayInfo.RoomOwnerID;
            set => _displayInfo.RoomOwnerID = _innerInfo.RoomOwnerID = value;
        }

        public string RoomName {
            get => _displayInfo.RoomName;
            set => _displayInfo.RoomName = value;
        }

        public RoomState RoomState {
            get => _displayInfo.RoomState;
            set => _displayInfo.RoomState = value;
        }

        public int MaxPeopleLimit {
            get => _displayInfo.MaxPeopleLimit;
            set => _displayInfo.MaxPeopleLimit = value;
        }

        public int WattingPeopleNumber {
            get => PlayerIDList.Count;
        }

        public IList<ChatInfo> ChatHistories {
            get => _innerInfo.ChatHistories;
        }

        public IReadOnlyList<int> PlayerIDList {
            get => _innerInfo.PlayerIDList;
        }

        internal Room(int roomID, int ownerID) {
            ID = roomID;
            OwnerID = ownerID;
        }

        public RoomDisplayInfo GetDisplayInfo() {
            return _displayInfo;
        }

        public RoomInnerInfo GetInnerInfo() {
            return _innerInfo;
        }

        public void AddPlayerID(int id, MessageChannel channel) {
            _innerInfo.PlayerIDList.Add(id);
            PlayerIDToChannelMapping.Add(id, channel);
            _displayInfo.CurrentPeopleNumber = PlayerIDList.Count;
        }

        public void RemovePlayerID(int id) {
            _innerInfo.PlayerIDList.Remove(id);
            PlayerIDToChannelMapping.Remove(id);
            _displayInfo.CurrentPeopleNumber = PlayerIDList.Count;
        }
    }
}
