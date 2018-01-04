using NetcodeIO.NET;
using ReliableNetcode;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityNetcodeIO;

public class Transport : MonoBehaviour
{
    private int updateTimeout = 1;
    protected NetcodeClient client;
    protected ReliableEndpoint endpoint;

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
        UnityNetcode.QuerySupport((supportStatus) =>
        {
            UnityNetcode.CreateClient(NetcodeIOClientProtocol.IPv4, (client) =>
            {
                this.client = client;

                connectToServer();
            });
        });
    }

    private void connectToServer()
    {
        var factory = new TokenFactory(0x1122334455667788L, _privateKey);
        byte[] connectToken = factory.GenerateConnectToken(new IPEndPoint[] { new IPEndPoint(IPAddress.Parse("192.168.1.70"), 12345) },
            30,
            5,
            1UL,
            1UL,
            new byte[256]);
        
        endpoint = new ReliableEndpoint
        {
            ReceiveCallback = (buffer, size) =>
            {
                var received = System.Text.Encoding.ASCII.GetString(buffer);
                print("Recived: " + received);
            },
            TransmitCallback = (buffer, size) =>
            {
                client.Send(buffer);
            }
        };

        client.Connect(connectToken, () =>
        {
            print("Connected to netcode.io server!");

            // add listener for network messages
            client.AddPayloadListener((client, packet) =>
            {
                endpoint.ReceivePacket(packet.PacketBuffer.InternalBuffer, packet.PacketBuffer.InternalBuffer.Length);
            });

            StartCoroutine(UpdateStatus());
            StartCoroutine(UpdateEndpoint());
        }, (err) =>
        {
            print("Failed to connect: " + err);
        });
    }
    
    private void ReceivePacket(NetcodeClient client, NetcodePacket packet)
    {
        endpoint.ReceivePacket(packet.PacketBuffer.InternalBuffer, packet.PacketBuffer.InternalBuffer.Length);
    }

    private IEnumerator UpdateStatus()
    {
        while (true)
        {
            client.QueryStatus((status) =>
            {
                print(string.Format("status: {0}({1})", status, (int)status));
            });

            yield return new WaitForSeconds(updateTimeout);
        }
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
