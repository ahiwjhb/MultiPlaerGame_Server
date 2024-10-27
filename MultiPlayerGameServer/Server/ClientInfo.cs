namespace MultiPlayerGame.Server
{
    public sealed class ClientInfo
    {
        public DateTime LastHeartbeatTime { get; internal set; }
    }
}