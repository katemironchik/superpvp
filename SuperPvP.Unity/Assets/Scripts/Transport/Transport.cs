using Assets.Scripts.Transport;
using NetcodeIO.NET;
using ReliableNetcode;
using SuperPvP.Core.Server;
using SuperPvP.Core.Server.Models;
using System.Collections;
using System.Net;
using UnityEngine;

public class Transport : MonoBehaviour
{
    private float updateTimeout = 1f;
    protected Client client;
    protected ReliableEndpoint endpoint;
    private int clientId;

    private ulong tick = 0;

    private ServerResponseProcessor processor;

    private static readonly byte[] _privateKey = new byte[]
    {
        0x60, 0x6a, 0xbe, 0x6e, 0xc9, 0x19, 0x10, 0xea,
        0x9a, 0x65, 0x62, 0xf6, 0x6f, 0x2b, 0x30, 0xe4,
        0x43, 0x71, 0xd6, 0x2c, 0xd1, 0x99, 0x27, 0x26,
        0x6b, 0x3c, 0x60, 0xf4, 0xb7, 0x15, 0xab, 0xa1,
    };

    // Use this for initialization
    void Start()
    {
        processor = gameObject.GetComponent<ServerResponseProcessor>();
        client = new Client();
        connectToServer();
    }

    void Update()
    {
        endpoint.Update();
    }

    public void SendPacketToServer(ServerGameObject change)
    {
        var buffer = PacketGenerator.CreateCommandPacket(tick, change);
        endpoint.SendMessage(buffer, buffer.Length, QosType.Unreliable);
    }

    private void connectToServer()
    {
        var factory = new TokenFactory(0x1122334455667788L, _privateKey);
        clientId = Random.Range(1, 100);
        string ip = "192.168.1.70";
        ip = "54.243.1.231";
        byte[] connectToken = factory.GenerateConnectToken(
            new IPEndPoint[]
            {
                new IPEndPoint(IPAddress.Parse(ip), 12345),
                new IPEndPoint(IPAddress.Parse("172.31.26.109"), 12345)
            },
            30,
            5,
            1UL,
            (ulong)clientId,
            new byte[256]);

        client.Connect(connectToken);
        client.OnStateChanged += UpdateStatus;
        client.OnMessageReceived += (payload, payloadSize) =>
        {
            endpoint.ReceivePacket(payload, payloadSize);
        };

        endpoint = new ReliableEndpoint
        {
            ReceiveCallback = (buffer, size) =>
            {
                var packet = PacketGenerator.DecodePacket(buffer);
                if (packet.TickId > tick)
                {
                    processor.Process(packet);
                    tick = packet.TickId;
                }
                else
                {
                    print("Skip: " + packet);
                }
            },
            TransmitCallback = (payload, payloadSize) =>
            {
                client.Send(payload, payloadSize);
            }
        };        
    }

    private void UpdateStatus(ClientState state)
    {
        print(string.Format("status: {0}({1})", state.ToString(), (int)state));
    }

    private IEnumerator UpdateEndpoint()
    {
        while (true)
        {
            endpoint.Update();
            yield return new WaitForSeconds(updateTimeout);
        }
    }
}
