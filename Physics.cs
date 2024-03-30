using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realms
{
    internal class Physics
    {
        public struct AABB
        {
            public Vector2 center;
            public Vector2 halfSize;

            public AABB(Vector2 center, Vector2 halfSize)
            {
                this.center = center;
                this.halfSize = halfSize;
            }

            public bool Overlaps(AABB other)
            {
                if (Math.Abs(center.X - other.center.X) > halfSize.X + other.halfSize.X) return false;
                if (Math.Abs(center.Y - other.center.Y) > halfSize.Y + other.halfSize.Y) return false;
                return true;
            }
        }

        
    }
}
