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
using System.Collections.Generic;

namespace SharpGameLib.Collision
{
    public struct LineSegment : IComparer<Vector2>
    {
        private static readonly Vector2 NullVector = new Vector2(float.NaN, float.NaN);

        public LineSegment(Vector2 p0, Vector2 dv)
        {
            this.P0 = p0;
            this.Dvec = dv;

            this.P1 = p0 + dv;
            this.Length = dv.Length();
        }

        public Vector2 P0;

        public Vector2 P1;

        public Vector2 Dvec;

        public float Length { get; }

        public bool Contains(Vector2 point)
        {
            return this.Contains(point.X, point.Y);
        }

        public bool Contains(float x, float y)
        {
            var x0 = P0.X;
            var y0 = P0.Y;
            var x1 = P1.X;
            var y1 = P1.Y;
            var inRangeX = x >= Math.Min(x0, x1) && x <= Math.Max(x0, x1);
            var inRangeY = y >= Math.Min(y0, y1) && y <= Math.Max(y0, y1);
            if (MathUtils.IsZero(Dvec.X))
            {
                return inRangeX && inRangeY;
            }

            var slope = Dvec.Y / Dvec.X;
            var inLine = MathUtils.AreEqual(y, slope * (x - x0) + y0);
                    
            return inLine && inRangeX && inRangeY;
        }

        public Vector2 Intersection(LineSegment seg)
        {
            var seg0 = this.Coefficients();
            var seg1 = seg.Coefficients();
            var result = MathUtils.LinSolve(seg0[0], seg0[1], seg0[2], seg1[0], seg1[1], seg1[2]);
            if (result != null)
            {
                return new Vector2((float)result.Item1, (float)result.Item2);
            }

            return NullVector;
        }

        /// <summary>
        /// Computes the alpha (between 0 and 1) time values at which
        /// the intersection of this line segment and 'seg' occur. The
        /// result is of the form [t0, t1] where t0 is the time value for
        /// this line and t1 is the time value for 'seg'. If no solution exists
        /// within the line segment, t0 or t1 (or both) will be NaN.
        /// </summary>
        /// <param name="seg">Seg.</param>
        public Vector2 IntersectionTime(LineSegment seg)
        {
            var a = this.Dvec.X;
            var b = -seg.Dvec.X;
            var c = seg.P0.X - this.P0.X;
            var w = this.Dvec.Y;
            var u = -seg.Dvec.Y;
            var v = seg.P0.Y - this.P0.Y;
            var result = MathUtils.LinSolve(a, b, c, w, u, v);
            var t0 = float.NaN;
            var t1 = float.NaN;
            if (result != null && Math.Abs(result.Item1) <= 1)
            {
                t0 = (float)Math.Abs(result.Item1);
            }

            if (result != null && Math.Abs(result.Item2) <= 1)
            {
                t1 = (float)Math.Abs(result.Item2);
            }

            return new Vector2(t0, t1);
        }

        public bool Intersects(LineSegment seg)
        {
            var intersec = this.Intersection(seg);
            return this.Contains(intersec.X, intersec.Y);
        }

        /// <summary>
        /// Alpha value for the given point on the line segment (in other words,
        /// the fraction of the overall length of the segment). If the given point
        /// does not lie on the segment, the result is undefined.
        /// </summary>
        /// <returns></returns>
        /// <param name="pt">Point.</param>
        public float AlphaFor(Vector2 pt)
        {
            return (pt - this.P0).Length() / this.Length;
        }

        public LineSegment Apply(float alpha)
        {
            return new LineSegment(this.P0, this.Dvec * alpha);
        }

        public int Compare(Vector2 point1, Vector2 point2)
        {
            return Vector2.Distance(this.P0, point1).CompareTo(Vector2.Distance(this.P0, point2));
        }

        public bool IsVertical()
        {
            return MathUtils.IsZero(this.Dvec.X);
        }

        public bool IsHorizontal()
        {
            return MathUtils.IsZero(this.Dvec.Y);
        }

        public bool IsPoint()
        {
            return MathUtils.IsZero(this.Dvec);
        }

        public float[] Coefficients()
        {
            var coeffs = new float[3];
            if (this.IsPoint())
            {
                return coeffs;
            }

            if (this.IsHorizontal())
            {
                coeffs[1] = 1;
                coeffs[2] = this.P1.Y;
                return coeffs;
            }

            if (this.IsVertical())
            {
                coeffs[0] = 1;
                coeffs[2] = this.P1.X;
                return coeffs;
            }

            var slope = Dvec.Y / Dvec.X;
            coeffs[0] = -slope;
            coeffs[1] = 1;
            coeffs[2] = slope * (0 - P0.X) + P0.Y;
            return coeffs;
        }
    }
}

