using Assets.Scripts.Configs;
using SuperPvP.Core.Server;
using SuperPvP.Core.Server.Models;
using UnityEngine;

public class ServerCommandSender : MonoBehaviour
{
    private Transport transport;
    private GameField field;
    
    // Use this for initialization
    void Start()
    {
        transport = GameObject.Find(GameObjects.ServerTransport).GetComponent<Transport>();
        field = GameObject.Find(GameObjects.GameField).GetComponent<GameField>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendPlayerMoveData(int i, int j)
    {
        var go = new ServerGameObject
        {
            Id = field.PlayerId,
            Position = new ServerPosition(i, j),
            Type = GameObjectType.Player
        };
        var packet = new TransportPacket(PacketType.Command, go);
        transport.SendPacketToServer(packet);
    }
}