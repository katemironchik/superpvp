using Newtonsoft.Json;
using System.Text;
using SuperPvP.Core.Server.Models;
using System.Collections.Generic;

namespace SuperPvP.Core.Server
{
    public class TransportPacket
    {
        public ulong TickId { get; set; }

        public PacketType Type { get; set; }

        public List<ServerGameObject> changes;

        public TransportPacket()
        {
            changes = new List<ServerGameObject>();
        }
    }
}
