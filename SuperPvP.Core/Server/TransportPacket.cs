﻿using Newtonsoft.Json;

namespace SuperPvP.Core.Server
{
    public class TransportPacket
    {
        private static char separator = '|';

        public ulong TickId { get; set; }

        public PacketType Type { get; set; }

        public string Data { get; set; }

        public TransportPacket(ulong tick, PacketType type, object data)
        {
            TickId = tick;
            Type = type;
            Data = JsonConvert.SerializeObject(data);
        }

        public TransportPacket(string recivedData)
        {
            var array = recivedData.Split(separator);
            TickId = ulong.Parse(array[0]);
            Type = (PacketType)int.Parse(array[1]);
            if (array.Length >= 2)
            {
                Data = array[3];
            }
        }

        public T Parse<T>()
        {
            return string.IsNullOrEmpty(Data) ? JsonConvert.DeserializeObject<T>(Data) : default(T);
        }

        public override string ToString()
        {
            var array = new string[] { TickId.ToString(), ((int)Type).ToString(), Data };            
            return string.Join(separator.ToString(), array);
        }
    }
}