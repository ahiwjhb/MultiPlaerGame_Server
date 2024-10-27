using Logger;
using System.Net;
using Network.Protocol;
using System.Net.Sockets;
using Network.MessageChannel;
using MultiPlayerGame.ServicesInjection;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private readonly LinkedList<MessageChannel> _clientChannels;

        private readonly Dictionary<MessageChannel, ClientInfo> _clientInfoMapping;

        private bool _isListening = false;

        public ILogger Logger { get; set; }

        public Server(ILogger logger) {
            Logger = logger;
            _clientChannels = new LinkedList<MessageChannel>();
            _clientInfoMapping = new Dictionary<MessageChannel, ClientInfo>();
        }

        public void StartServer(int port) {
            Logger.Log("开始服务");
            StartCheckClientsHeartbeatThread();
            AcceptClientConnection(port);
        }

        public void EndServer() {
            _isListening = false;
        }

        private void AcceptClientConnection(int port) {
            Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen();

            _isListening = true;
            while (_isListening) {
                Socket socket = listener.Accept();
                MessageChannel clientChannel = AddClientMessageChannel();
                clientChannel.StartListen(socket);

                Logger.LogFormat("接收到连接请求 ip地址{0}", clientChannel.ConnectPoint);
            }

            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
            listener.Dispose();
        }

        private MessageChannel AddClientMessageChannel() {
            var clientChannel = Services.GetService<MessageChannel>();
            clientChannel.AddMessageListener<HeartbeatPack>(HandleClientHeartbeat);
            clientChannel.AddMessageListener<LoginRequest>(HandleClientLoginRequest);
            clientChannel.AddMessageListener<RegisterRequest>(HandleClientRegisterRequest);
            clientChannel.AddMessageListener<CreateRoomRequest>(HandleClientCreatRoomRequest);
            clientChannel.AddMessageListener<SearchRoomRequest>(HandleClientSearchRoomRequest);
            clientChannel.AddMessageListener<JoinRoomRequest>(HandleClientJoinRoomRequest);
            clientChannel.AddMessageListener<UserInfoRequest>(HandleClientQueryUserInfoRequest);
            clientChannel.AddMessageListener<ExitRoomRequest>(HandleClientExitRoomRequest);
            clientChannel.AddMessageListener<SyncPlayerPositionRequest>(HandleClientSyncPlayerPositionRequest);
            clientChannel.AddMessageListener<StartGameRequest>(HandleClientStartGameRequest);
            clientChannel.AddMessageListener<FireRequest>(HandleClientFireRequest);

            lock (_clientChannels) {
                _clientChannels.AddLast(clientChannel);
            }
            lock (_clientInfoMapping) {
                var clientInfo = new ClientInfo() {
                    LastHeartbeatTime = DateTime.Now
                };
                _clientInfoMapping.Add(clientChannel, clientInfo);
            }
            return clientChannel;
        }

        private void RemoveClientMessageChannel(LinkedListNode<MessageChannel> node) {
            MessageChannel clientChannel = node.Value;
            lock (_clientChannels) {
                _clientChannels.Remove(node);
            }
            lock (_clientInfoMapping) {
                _clientInfoMapping.Remove(clientChannel);
            }
            clientChannel.Dispose();

            Logger.LogFormat("ip地址{0} 客户端断开连接", clientChannel.ConnectPoint);
        }

        private void StartCheckClientsHeartbeatThread() {
            new Thread(PollingClientHeartbeat) {
                IsBackground = true
            }.Start();
        }

        private void PollingClientHeartbeat() {
            const double checkHeatbeatCircelTime = 2f;
            try {
                while (_isListening) {
                    CheckHeatbeat();
                    Thread.Sleep((int)(checkHeatbeatCircelTime * 1000));
                }
            }
            catch (Exception e) {
                Logger.Error("心跳包轮询异常！", e);
            }

            void CheckHeatbeat() {
                const double maxHeartbeatInterval = 30f;
                LinkedListNode<MessageChannel>? iterator = _clientChannels.First;
                while (iterator != null) {
                    MessageChannel clientChannel = iterator.Value;
                    ClientInfo clientInfo = _clientInfoMapping[clientChannel];
                    double elapsedTime = (DateTime.Now - clientInfo.LastHeartbeatTime).TotalSeconds;
                    if (elapsedTime > maxHeartbeatInterval || clientChannel.Connected == false) {
                        var temp = iterator;
                        iterator = iterator.Next;
                        RemoveClientMessageChannel(temp);
                    }
                    else {
                        iterator = iterator.Next;
                    }
                }
            }
        }
    }
}
