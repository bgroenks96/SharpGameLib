/*
 * Copyright (C) 2016-2017 (See COPYRIGHT.txt for holders)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

﻿using System;
using Microsoft.Xna.Framework;
using System.Text;

namespace SharpGameLib.Collision
{
    public class CollisionEdge
    {
        public static readonly CollisionEdge Top = new CollisionEdge(TopFlag);

        public static readonly CollisionEdge Bottom = new CollisionEdge(BottomFlag);

        public static readonly CollisionEdge Left = new CollisionEdge(LeftFlag);

        public static readonly CollisionEdge Right = new CollisionEdge(RightFlag);

        public static readonly CollisionEdge None = new CollisionEdge(0);

        private const int TopFlag = 0x10;

        private const int BottomFlag = 0x20;

        private const int LeftFlag = 0x40;

        private const int RightFlag = 0x80;

        private readonly int bitmask = 0;

        private CollisionEdge(int bitmask)
        {
            this.bitmask = bitmask;
        }

        public static CollisionEdge From(CollisionEdge edge1, CollisionEdge edge2)
        {
            return new CollisionEdge(edge1.bitmask | edge2.bitmask);
        }

        public CollisionEdge CombineWith(CollisionEdge edge)
        {
            return From(this, edge);
        }

        public CollisionEdge Except(CollisionEdge edge)
        {
            return new CollisionEdge(this.bitmask ^ edge.bitmask);
        }

        public CollisionEdge Complement()
        {
            var reverse = CollisionEdge.None;
            if (this.IsRight())
            {
                reverse = reverse.CombineWith(Left);
            }

            if (this.IsLeft())
            {
                reverse = reverse.CombineWith(Right);
            }

            if (this.IsTop())
            {
                reverse = reverse.CombineWith(Bottom);
            }

            if (this.IsBottom())
            {
                reverse = reverse.CombineWith(Top);
            }

            return reverse;
        }

        public bool IsLeft()
        {
            return (this.bitmask & LeftFlag) != 0;
        }

        public bool IsRight()
        {
            return (this.bitmask & RightFlag) != 0;
        }

        public bool IsTop()
        {
            return (this.bitmask & TopFlag) != 0;
        }

        public bool IsBottom()
        {
            return (this.bitmask & BottomFlag) != 0;
        }

        public bool IsNone()
        {
            return this.bitmask == 0;
        }

        public override bool Equals(object obj)
        {
            return this.bitmask == (obj as CollisionEdge)?.bitmask;
        }

        public override int GetHashCode()
        {
            return 271 * (0x00FF ^ (int)this.bitmask);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder($"[{this.bitmask}]:");
            if (this.IsLeft())
            {
                stringBuilder.Append("  Left");
            }

            if (this.IsRight())
            {
                stringBuilder.Append("  Right");
            }

            if (this.IsTop())
            {
                stringBuilder.Append("  Top");
            }

            if (this.IsBottom())
            {
                stringBuilder.Append("  Bottom");
            }

            if (this.IsNone())
            {
                stringBuilder.Append("  None");
            }

            return stringBuilder.ToString();
        }
    }
}

