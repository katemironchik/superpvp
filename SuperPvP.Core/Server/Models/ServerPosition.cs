using System;
using System.Collections.Generic;

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

        public static bool IsNullOrEmpty(ServerPosition p)
        {
            return p == null || (p.I == 0 && p.J == 0);
        }

        public static ServerPosition operator - (ServerPosition begin, ServerPosition end)
        {
            return new ServerPosition
            {
                I = end.I - begin.I,
                J = end.J - begin.J
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var sp = (ServerPosition) obj;
            return I == sp.I && J == sp.J;
        }

        public void AddVector (ServerPosition p)
        {
            I += p.I;
            J += p.J;
        }

        public ServerPosition Normalize()
        {
            int i = I == 0 ? 0 : Math.Sign(I);
            int j = J == 0 ? 0 : Math.Sign(J);

            return new ServerPosition(i, j);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", I, J);
        }
    }
}
