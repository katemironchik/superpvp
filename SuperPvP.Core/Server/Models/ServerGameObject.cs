namespace SuperPvP.Core.Server.Models
{
    public class ServerGameObject
    {
        public int Id { get; set; }

        public ServerPossition Possition { get; set; }

        public GameObjectType Type { get; set; }
    }
}
