using Newtonsoft.Json;
using System.Text;

namespace SuperPvP.Core.Server
{
    public class TransportPacket
    {
        private static Encoding encoding = Encoding.ASCII;
        private static char separator = '|';

        public ulong TickId { get; private set; }

        public PacketType Type { get; set; }

        public string Data { get; set; }

        public TransportPacket(PacketType type, object data)
        {
            Type = type;
            Data = JsonConvert.SerializeObject(data);
        }

        public TransportPacket(byte[] buffer) : this(encoding.GetString(buffer))
        {
        }

        public TransportPacket(string recivedData)
        {
            var array = recivedData.Split(separator);
            TickId = ulong.Parse(array[0]);
            Type = (PacketType)int.Parse(array[1]);
            if (array.Length >= 2)
            {
                Data = array[2].Replace("\\", string.Empty).Trim('"');
            }
        }

        public void SetTickId(ulong tickId)
        {
            TickId = tickId;
        }

        public T Parse<T>()
        {
            return !string.IsNullOrEmpty(Data) ? JsonConvert.DeserializeObject<T>(Data) : default(T);
        }

        public override string ToString()
        {
            var array = new string[] { TickId.ToString(), ((int)Type).ToString(), Data };            
            return string.Join(separator.ToString(), array);
        }
        
        public byte[] ToByteArray()
        {
            return encoding.GetBytes(ToString());
        }
    }
}
