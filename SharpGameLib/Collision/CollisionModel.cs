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
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SharpGameLib.Collision
{
    public struct CollisionModel
    {
        // private readonly Vector2 vec;

        private readonly LineSegment[] moveEdges;

        public CollisionModel(RectangleF r0, Vector2 vec)
        {
            this.R0 = r0;
            this.R1 = MathUtils.ApplyTo(r0, vec);
            this.Dv = vec;

            // create move edges from corners where the endpoint is not in the initial bounds
            this.moveEdges = MathUtils.Corners(r0).Select(p => new LineSegment(p, vec)).Where(s => !r0.Contains(s.P1)).ToArray();
        }

        public CollisionModel(RectangleF r0, RectangleF r1)
        {
            this.R0 = r0;
            this.R1 = r1;
            this.Dv = new Vector2(r1.X - r0.X, r1.Y - r0.Y);

            // create move edges from corners where the endpoint is not in the initial bounds
            var dv = this.Dv;
            this.moveEdges = MathUtils.Corners(r0).Select(p => new LineSegment(p, dv)).Where(s => !r0.Contains(s.P1)).ToArray();
        }

        public RectangleF R0 { get; }

        public RectangleF R1 { get; }

        public Vector2 Dv { get; }

        public bool IsInside(RectangleF bounds)
        {
            return bounds.Contains(this.R0) && bounds.Contains(this.R1);
        }

        public bool Intersects(CollisionModel other)
        {
            if (R0.Intersects(other.R0) || R0.Intersects(other.R1))
            {
                return true;
            }

            if (R1.Intersects(other.R0) || R1.Intersects(other.R1))
            {
                return true;
            }

            return this.moveEdges.Any(e0 => other.moveEdges.Any(e0.Intersects));
        }

        public bool DoesNotIntersect(CollisionModel other)
        {
            return !this.Intersects(other);
        }

        public RectangleF Intersection(CollisionModel other)
        {
            var rself = this.R1;
            var rother = other.R1;
            var overlap = RectangleF.Intersect(rself, rother);

            // return intersection rectangle, or empty rectangle if there is no intersection
            if (overlap != RectangleF.Empty || !this.Intersects(other))
            {
                return overlap;
            }

            return RectangleF.Empty;
        }

        public CollisionMoment GetCollision(CollisionModel other)
        {
            if (this.DoesNotIntersect(other))
            {
                return CollisionMoment.NullCollision;
            }

            var overlap = RectangleF.Intersect(this.R1, other.R1);
            var thisDv = MathUtils.Abs(this.Dv);
            var len = thisDv.Length();
            var dx = Math.Min(overlap.Width, thisDv.X);
            var dy = Math.Min(overlap.Height, thisDv.Y);
            var dvec = new Vector2(dx, dy);

            var edges = this.DetermineEdges(other);
            var alpha = len > 0 ? (1 - dvec.Length() / thisDv.Length()) : 1;
            return new CollisionMoment(edges.Item1, edges.Item2, alpha);
        }

        private Tuple<CollisionEdge, CollisionEdge> DetermineEdges(CollisionModel other)
        {
            var thisEdge = CollisionEdge.None;
            var otherEdge = CollisionEdge.None;
			if (other.R1.Width <= 0 || other.R1.Height <= 0)
			{
				return new Tuple<CollisionEdge, CollisionEdge>(thisEdge, otherEdge);
			}

            var otherEdgeMapping = new Dictionary<LineSegment, CollisionEdge>()
            {
                { other.R1.LeftEdge, CollisionEdge.Left },
                { other.R1.RightEdge, CollisionEdge.Right },
                { other.R1.BottomEdge, CollisionEdge.Bottom },
                { other.R1.TopEdge, CollisionEdge.Top }
            };

            var intersections = new List<IntersectionPoint>();
            foreach (var moveEdge in this.moveEdges)
            {
                foreach (var edge in otherEdgeMapping.Keys)
                {
                    var p = moveEdge.Intersection(edge);
                    if (MathUtils.IsVectorNaN(p) || MathUtils.IsVectorInfinite(p))
                    {
                        continue;
                    }

                    var dist = Vector2.Distance(moveEdge.P0, p);
                    var otherIntersectionEdge = otherEdgeMapping[edge];
                    CollisionEdge thisIntersectionEdge;
                    if (otherIntersectionEdge.IsBottom() || otherIntersectionEdge.IsTop())
                    {
                        var top = this.R1.TopEdge;
                        var bottom = this.R1.BottomEdge;
                        thisIntersectionEdge = p.Distance(top) < p.Distance(bottom) ? CollisionEdge.Top : CollisionEdge.Bottom;
                    }
                    else
                    {
                        var left = this.R1.TopEdge;
                        var right = this.R1.BottomEdge;
                        thisIntersectionEdge = p.Distance(left) < p.Distance(right) ? CollisionEdge.Left : CollisionEdge.Right;
                    }

                    intersections.Add(new IntersectionPoint(dist, thisIntersectionEdge, otherIntersectionEdge));
                }
            }

            intersections = intersections.OrderBy(t => t.Distance).ToList();
                
            if (intersections.Any())
            {
                var min = intersections.First();
                thisEdge = min.ThisEdge;
                otherEdge = min.OtherEdge;
                if (thisEdge == CollisionEdge.Bottom && otherEdge == CollisionEdge.Bottom && MathUtils.AreEqual(32, other.R0.Height))
                {
                }
            }

            return new Tuple<CollisionEdge, CollisionEdge>(thisEdge, otherEdge);
        }

        struct IntersectionPoint
        {
            internal IntersectionPoint(float dist, CollisionEdge thisEdge, CollisionEdge otherEdge)
            {
                this.Distance = dist;
                this.ThisEdge = thisEdge;
                this.OtherEdge = otherEdge;
            }

            internal float Distance { get; }

            internal CollisionEdge ThisEdge { get; }

            internal CollisionEdge OtherEdge { get; }
        }
    }
}

