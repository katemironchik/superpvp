using Assets.Scripts.Configs;
using SuperPvP.Core.Server;
using SuperPvP.Core.Server.Models;
using System.Collections.Generic;
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
            var data = packet.Parse<List<ServerGameObject>>();
            field.RefreshFieldFromServer(data);
        }

        private void SetPlayerIdentifier(TransportPacket packet)
        {
            var data = packet.Parse<int>();
            field.SetPlayerIdentifier(data);
        }
    }
}
