using NetcodeIO.NET;
using SuperPvP.Core.Server.Models;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityNetcodeIO;

public class Transport : MonoBehaviour
{
    public string outputText;

    protected NetcodeClient client;

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
        
        client.Connect(connectToken, () =>
        {
            print("Connected to netcode.io server!");

            // add listener for network messages
            client.AddPayloadListener(ReceivePacket);

            // do stuff
            StartCoroutine(updateStatus());
            StartCoroutine(doStuff());
        }, (err) =>
        {
            print("Failed to connect: " + err);
        });
    }

    int received = 0;
    private void ReceivePacket(NetcodeClient client, NetcodePacket packet)
    {
        received++;
        var packetBuffer = packet.PacketBuffer.ToString();
        print("Recived: " + packetBuffer);
    }

    private IEnumerator updateStatus()
    {
        while (true)
        {
            client.QueryStatus((status) =>
            {
                print(string.Format("status: {0}({1})", status, (int)status));
            });

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator doStuff()
    {
        int sent = 0;

        while (true)
        {
            if (client.Status == NetcodeClientStatus.Connected)
            {
                // send a packet
                var packetStr = "pkt " + sent + "! " + System.DateTime.Now.ToString();
                sent++;
                
                var packetBuffer = System.Text.Encoding.ASCII.GetBytes(packetStr);

                client.Send(packetBuffer);
            }

            yield return new WaitForSeconds(1);
        }
    }
}
