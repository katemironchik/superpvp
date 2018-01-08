using Assets.Scripts.Configs;
using SuperPvP.Core.Server;
using UnityEngine;

namespace Assets.Scripts.Transport
{
    public class ServerResponseProcessor : MonoBehaviour
    {
        private GameField field;

        void Start()
        {
            field = GameObject.Find(GameObjects.GameField).GetComponent<GameField>();
        }

        public void Process(TransportPacket packet)
        {
            switch (packet.Type)
            {
                case PacketType.Initialize:
                    SetPlayerIdentifier(packet);
                    return;
                case PacketType.Update:
                    RenderField(packet);
                    return;
            }
        }

        private void RenderField(TransportPacket packet)
        {
            field.Map = packet.changes;
        }

        private void SetPlayerIdentifier(TransportPacket packet)
        {
            field.SetPlayerIdentifier(packet.changes[0].Id);
        }
    }
}
