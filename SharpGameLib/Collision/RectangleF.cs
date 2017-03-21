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
    /// <summary>
    /// Adaptation of MonoGame's <see cref="RectangleF"/> type to use floating point values instead
    /// of integers.
    /// </summary>
    public struct RectangleF : IEquatable<RectangleF>
    {
        #region Private Fields

        private static RectangleF emptyRectangleF = new RectangleF();

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of the top-left corner of this <see cref="RectangleF"/>.
        /// </summary>
        public float X;

        /// <summary>
        /// The y coordinate of the top-left corner of this <see cref="RectangleF"/>.
        /// </summary>
        public float Y;

        /// <summary>
        /// The width of this <see cref="RectangleF"/>.
        /// </summary>
        public float Width;

        /// <summary>
        /// The height of this <see cref="RectangleF"/>.
        /// </summary>
        public float Height;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a <see cref="RectangleF"/> with X=0, Y=0, Width=0, Height=0.
        /// </summary>
        public static RectangleF Empty
        {
            get { return emptyRectangleF; }
        }

        /// <summary>
        /// Returns the x coordinate of the left edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Left
        {
            get { return this.X; }
        }

        /// <summary>
        /// Returns the x coordinate of the right edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Right
        {
            get { return (this.X + this.Width); }
        }

        /// <summary>
        /// Returns the y coordinate of the top edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Top
        {
            get { return this.Y; }
        }

        /// <summary>
        /// Returns the y coordinate of the bottom edge of this <see cref="RectangleF"/>.
        /// </summary>
        public float Bottom
        {
            get { return (this.Y + this.Height); }
        }

        /// <summary>
        /// Whether or not this <see cref="RectangleF"/> has a <see cref="Width"/> and
        /// <see cref="Height"/> of 0, and a <see cref="Location"/> of (0, 0).
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((((this.Width == 0) && (this.Height == 0)) && (this.X == 0)) && (this.Y == 0));
            }
        }

        public LineSegment TopEdge
        {
            get
            {
                var topCorner = new Vector2(this.Left, this.Top);
                var xvec = new Vector2(this.Width, 0);
                return new LineSegment(topCorner, xvec);
            }
        }

        public LineSegment BottomEdge
        {
            get
            {
                var bottomCorner = new Vector2(this.Left, this.Bottom);
                var xvec = new Vector2(this.Width, 0);
                return new LineSegment(bottomCorner, xvec);
            }
        }

        public LineSegment LeftEdge
        {
            get
            {
                var topCorner = new Vector2(this.Left, this.Top);
                var yvec = new Vector2(0, this.Height);
                return new LineSegment(topCorner, yvec);
            }
        }

        public LineSegment RightEdge
        {
            get
            {
                var topCorner = new Vector2(this.Right, this.Top);
                var yvec = new Vector2(0, this.Height);
                return new LineSegment(topCorner, yvec);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="RectangleF"/> struct, with the specified
        /// position, width, and height.
        /// </summary>
        /// <param name="x">The x coordinate of the top-left corner of the created <see cref="RectangleF"/>.</param>
        /// <param name="y">The y coordinate of the top-left corner of the created <see cref="RectangleF"/>.</param>
        /// <param name="width">The width of the created <see cref="RectangleF"/>.</param>
        /// <param name="height">The height of the created <see cref="RectangleF"/>.</param>
        public RectangleF(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleF"/> struct, with the specified
        /// location and size.
        /// </summary>
        /// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="RectangleF"/>.</param>
        /// <param name="size">The width and height of the created <see cref="RectangleF"/>.</param>
        public RectangleF(Point location, Point size)
        {
            this.X = location.X;
            this.Y = location.Y;
            this.Width = size.X;
            this.Height = size.Y;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Compares whether two <see cref="RectangleF"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="RectangleF"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="RectangleF"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(RectangleF a, RectangleF b)
        {
            return ((MathUtils.AreEqual(a.X, b.X) && MathUtils.AreEqual(a.Y, b.Y) && MathUtils.AreEqual(a.Width, b.Width) && MathUtils.AreEqual(a.Height, b.Height)));
        }

        /// <summary>
        /// Compares whether two <see cref="RectangleF"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="RectangleF"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="RectangleF"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return !(a == b);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the Point to check for containment.</param>
        /// <param name="y">The y coordinate of the Point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(float x, float y)
        {
            return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Point value)
        {
            return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="RectangleF"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref Point value, out bool result)
        {
            result = ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y <= (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Vector2 value)
        {
            return ((((this.X <= value.X) && (value.X <= (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y <= (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="RectangleF"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref Vector2 value, out bool result)
        {
            result = ((((this.X <= value.X) && (value.X <= (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y <= (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="RectangleF"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The <see cref="RectangleF"/> to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="RectangleF"/>'s bounds lie entirely inside this <see cref="RectangleF"/>; <c>false</c> otherwise.</returns>
        public bool Contains(RectangleF value)
        {
            return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="RectangleF"/> lies within the bounds of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The <see cref="RectangleF"/> to check for inclusion in this <see cref="RectangleF"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="RectangleF"/>'s bounds lie entirely inside this <see cref="RectangleF"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref RectangleF value, out bool result)
        {
            result = ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is RectangleF) && this == ((RectangleF)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="other">The <see cref="RectangleF"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(RectangleF other)
        {
            return this == other;
        }

        /// <summary>
        /// Gets the hash code of this <see cref="RectangleF"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="RectangleF"/>.</returns>
        public override int GetHashCode()
        {
            double x = this.X;
            double y = this.Y;
            double wt = this.Width;
            double ht = this.Height;
            return ((int)(x * 1E8) ^ (int)(y * 1E8) ^ (int)(wt * 1E8) ^ (int)(ht * 1E8));
        }

        /// <summary>
        /// Adjusts the edges of this <see cref="RectangleF"/> by specified horizontal and vertical amounts. 
        /// </summary>
        /// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
        /// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }

        /// <summary>
        /// Gets whether or not the other <see cref="RectangleF"/> Intersects with this RectangleF.
        /// </summary>
        /// <param name="value">The other RectangleF for testing.</param>
        /// <returns><c>true</c> if other <see cref="RectangleF"/> Intersects with this RectangleF; <c>false</c> otherwise.</returns>
        public bool Intersects(RectangleF value)
        {
            return value.Left < Right &&
            Left < value.Right &&
            value.Top < Bottom &&
            Top < value.Bottom;
        }
            

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that contains overlapping region of two other RectangleFs.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <returns>Overlapping region of the two RectangleFs.</returns>
        public static RectangleF Intersect(RectangleF value1, RectangleF value2)
        {
            RectangleF RectangleF;
            Intersect(ref value1, ref value2, out RectangleF);
            return RectangleF;
        }

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that contains overlapping region of two other RectangleFs.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <param name="result">Overlapping region of the two RectangleFs as an output parameter.</param>
        public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            if (value1.Intersects(value2))
            {
                float right_side = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                float left_side = Math.Max(value1.X, value2.X);
                float top_side = Math.Max(value1.Y, value2.Y);
                float bottom_side = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                result = new RectangleF(left_side, top_side, right_side - left_side, bottom_side - top_side);
            }
            else
            {
                result = new RectangleF(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="RectangleF"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="RectangleF"/>.</param>
        public void Offset(float offsetX, float offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="RectangleF"/>.</param>
        public void Offset(Point amount)
        {
            X += amount.X;
            Y += amount.Y;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="RectangleF"/>.</param>
        public void Offset(Vector2 amount)
        {
            X += (float)amount.X;
            Y += (float)amount.Y;
        }

        public Vector2 Intersection(LineSegment seg)
        {
            var intersections = new List<Vector2>();
            var rectSegments = this.ToLineSegments();
            foreach (var rseg in rectSegments)
            {
                var intersec = seg.Intersection(rseg);
                if (this.Contains((int)intersec.X, (int)intersec.Y))
                {
                    intersections.Add(intersec);
                }
            }

            // return the first intersection point along the line segment, or a NaN vector if no intersections exist
            return intersections.Any() ? intersections.OrderBy(v => v, seg).First() : new Vector2(float.NaN, float.NaN);
        }

        public LineSegment[] ToLineSegments()
        {
            var segments = new LineSegment[4];
            var location = new Vector2(this.X, this.Y);
            var wtVec = new Vector2(this.Width, 0);
            var htVec = new Vector2(0, this.Height);
            segments[0] = new LineSegment(location, wtVec);
            segments[1] = new LineSegment(location + wtVec, htVec);
            segments[2] = new LineSegment(location, htVec);
            segments[3] = new LineSegment(location + htVec, wtVec);
            return segments;
        }

        public Rectangle ToIntegerRect()
        {
            return new Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
        }

        public bool TopContains(Vector2 point)
        {
            return this.TopEdge.Contains(point);
        }

        public bool BottomContains(Vector2 point)
        {
            return this.BottomEdge.Contains(point);
        }

        public bool LeftContains(Vector2 point)
        {
            return this.LeftEdge.Contains(point);
        }

        public bool RightContains(Vector2 point)
        {
            return this.RightEdge.Contains(point);
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="RectangleF"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Width:[<see cref="Width"/>] Height:[<see cref="Height"/>]}
        /// </summary>
        /// <returns><see cref="String"/> representation of this <see cref="RectangleF"/>.</returns>
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        }

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that completely contains two other RectangleFs.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <returns>The union of the two RectangleFs.</returns>
        public static RectangleF Union(RectangleF value1, RectangleF value2)
        {
            float x = Math.Min(value1.X, value2.X);
            float y = Math.Min(value1.Y, value2.Y);
            return new RectangleF(x, y,
                Math.Max(value1.Right, value2.Right) - x,
                Math.Max(value1.Bottom, value2.Bottom) - y);
        }

        /// <summary>
        /// Creates a new <see cref="RectangleF"/> that completely contains two other RectangleFs.
        /// </summary>
        /// <param name="value1">The first <see cref="RectangleF"/>.</param>
        /// <param name="value2">The second <see cref="RectangleF"/>.</param>
        /// <param name="result">The union of the two RectangleFs as an output parameter.</param>
        public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
        {
            result.X = Math.Min(value1.X, value2.X);
            result.Y = Math.Min(value1.Y, value2.Y);
            result.Width = Math.Max(value1.Right, value2.Right) - result.X;
            result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
        }

        public static RectangleF From(Rectangle rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        #endregion
    }
}

