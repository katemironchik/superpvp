namespace SuperPvP.Core.Server.Models
{
    public class ServerGameObject
    {
        public int Id { get; set; }

        public ServerPosition Position { get; set; }

        public GameObjectType Type { get; set; }
    }
}
