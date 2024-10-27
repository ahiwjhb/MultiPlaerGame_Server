using Network.MessageChannel;
using Network.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientHeartbeat(MessageChannel sender, HeartbeatPack _) {
            ClientInfo clientInfo = _clientInfoMapping[sender];
            clientInfo.LastHeartbeatTime = DateTime.Now;
        }

    }
}
