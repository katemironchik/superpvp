using System;
using System.Collections.Generic;
using FlatBuffers;
using SuperPvp.Core.Transport;
using SuperPvP.Core.Server.Models;

namespace SuperPvP.Core.Server
{
    public class PacketGenerator
    {
        public static KeyValuePair<int, byte[]> CreateInitPacket(ulong tickId, int id,  int i, int j)
        {
            var builder = new FlatBufferBuilder(1);

            Packet.StartDataVector(builder, 1);
            ObjectChange.CreateObjectChange(builder, id, ObjectType.Player, i, j);
            var dataOffset = builder.EndVector();

            Packet.StartPacket(builder);
            Packet.AddTickId(builder, tickId);
            Packet.AddType(builder, SuperPvp.Core.Transport.PacketType.Initialize);
            Packet.AddData(builder, dataOffset);

            var packet = Packet.EndPacket(builder);
            builder.Finish(packet.Value);

            return new KeyValuePair<int, byte[]>(builder.DataBuffer.Position, builder.DataBuffer.Data);
        }

        public static KeyValuePair<int, byte[]> CreateUpdatePacket(ulong tickId, List<ServerGameObject> changes)
        {
            var builder = new FlatBufferBuilder(1);

            Packet.StartDataVector(builder, changes.Count);
            foreach (var change in changes)
            {
                var type = change.Type == GameObjectType.Player ? ObjectType.Player : ObjectType.Drug;
                ObjectChange.CreateObjectChange(builder, change.Id, type, change.Position.I, change.Position.J);
            }
            var dataOffset = builder.EndVector();

            Packet.StartPacket(builder);
            Packet.AddTickId(builder, tickId);
            Packet.AddType(builder, SuperPvp.Core.Transport.PacketType.Update);
            Packet.AddData(builder, dataOffset);

            var packet = Packet.EndPacket(builder);
            builder.Finish(packet.Value);

            return new KeyValuePair<int, byte[]>(builder.DataBuffer.Position, builder.DataBuffer.Data);
        }

        public static KeyValuePair<int, byte[]> CreateCommandPacket(ulong tickId, ServerGameObject targetState)
        {
            var builder = new FlatBufferBuilder(1);

            Packet.StartDataVector(builder, 1);
            ObjectChange.CreateObjectChange(builder,
                targetState.Id,
                ObjectType.Player,
                targetState.Position.I,
                targetState.Position.J);
            var dataOffset = builder.EndVector();

            Packet.StartPacket(builder);
            Packet.AddTickId(builder, tickId);
            Packet.AddType(builder, SuperPvp.Core.Transport.PacketType.Update);
            Packet.AddData(builder, dataOffset);

            var packet = Packet.EndPacket(builder);
            builder.Finish(packet.Value);

            return new KeyValuePair<int, byte[]>(builder.DataBuffer.Position, builder.DataBuffer.Data);
        }

        public static TransportPacket DecodePacket(byte[] data, int size)
        {
            var result = new TransportPacket();
            var packet = Packet.GetRootAsPacket(new ByteBuffer(data, size));
            result.TickId = packet.TickId;
            result.Type = (PacketType)((int)packet.Type);

            for (int i = 0; i < packet.DataLength; i++)
            {
                if (!packet.Data(i).HasValue) continue;
                var change = packet.Data(i).Value;
                var type = change.Type == ObjectType.Player ? GameObjectType.Player : GameObjectType.Drug;

                result.changes.Add(new ServerGameObject
                {
                    Id = change.Id,
                    Type = type,
                    Position = new ServerPosition(change.Position.I, change.Position.J)
                });
            }
            return result;
        }
    }
}