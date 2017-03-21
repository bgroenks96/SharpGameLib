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

namespace SharpGameLib.Collision
{
    public struct CollisionMoment
    {
        public static readonly CollisionMoment NullCollision = new CollisionMoment(
                                                                   CollisionEdge.None, 
                                                                   CollisionEdge.None,
                                                                   float.NaN);

        public CollisionMoment(CollisionEdge thisEdge, CollisionEdge otherEdge, float timeAlpha)
        {
            this.ThisEdge = thisEdge;
            this.OtherEdge = otherEdge;
            this.TimeAlpha = timeAlpha;
        }

        public CollisionEdge ThisEdge { get; }

        public CollisionEdge OtherEdge { get; }

        public float TimeAlpha { get; }

        public bool IsNullCollision()
        {
            return this.ThisEdge.Equals(CollisionEdge.None) &&
                this.OtherEdge.Equals(CollisionEdge.None) &&
                float.IsNaN(this.TimeAlpha);
        }

        /// <summary>
        /// Creates a new CollisionMoment with 'ThisEdge' and 'OtherEdge' reversed.
        /// </summary>
        /// <returns>The edge polarity.</returns>
        public CollisionMoment ReversePolarity()
        {
            return new CollisionMoment(this.OtherEdge, this.ThisEdge, this.TimeAlpha);
        }
    }
}

