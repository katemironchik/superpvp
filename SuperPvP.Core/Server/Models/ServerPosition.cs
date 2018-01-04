namespace SuperPvP.Core.Server.Models
{
    public class ServerPosition
    {
        public int I { get; set; }

        public int J { get; set; }

        public ServerPosition()
        {
        }

        public ServerPosition(int i, int j)
        {
            I = i;
            J = j;
        }
    }
}
