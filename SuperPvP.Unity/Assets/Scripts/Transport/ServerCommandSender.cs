using Assets.Scripts.Configs;
using SuperPvP.Core.Server;
using SuperPvP.Core.Server.Models;
using UnityEngine;

public class ServerCommandSender : MonoBehaviour
{
    private Transport transport;

    // Use this for initialization
    void Start()
    {
        transport = GameObject.Find(GameObjects.ServerTransport).GetComponent<Transport>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendPlayerMoveData(int i, int j)
    {
        var packet = new TransportPacket(PacketType.Command, new ServerPosition(i, j));
        transport.SendPacketToServer(packet);
    }
}