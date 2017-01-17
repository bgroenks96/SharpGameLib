using System;
using Microsoft.Xna.Framework;
using SharpGameLib.Collision;

namespace SharpGameLib
{
    public static class Vector2Ext
    {
        public static Vector2 NegateX(this Vector2 vec)
        {
            return new Vector2(-vec.X, vec.Y);
        }

        public static Vector2 NegateY(this Vector2 vec)
        {
            return new Vector2(vec.X, -vec.Y);
        }

        public static float Distance(this Vector2 vec, LineSegment line)
        {
            var vertical = new LineSegment(vec, new Vector2(0, 1));
            var intersec = line.Intersection(vertical);
            return Vector2.Distance(vec, intersec);
        }
    }
}

