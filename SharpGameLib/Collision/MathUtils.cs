using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Collision
{
    public static class MathUtils
    {
        public const float DefaultEpsilon = 1.0E-7f;

        public static bool AreEqual(double a, double b, double epsilon = DefaultEpsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        public static bool IsZero(double a, double epsilon = DefaultEpsilon)
        {
            return AreEqual(a, 0, epsilon);
        }

        public static bool IsZero(Vector2 vec, double epsilon = DefaultEpsilon)
        {
            return IsZero(vec.X, epsilon) && IsZero(vec.Y, epsilon);
        }

		public static float SelectMid(float a, float b, float c)
		{
			return Math.Max(Math.Min(a, b), Math.Min(Math.Max(a, b), c));
		}

        public static bool IsVectorNaN(Vector2 vec)
        {
            return float.IsNaN(vec.X) || float.IsNaN(vec.Y);
        }

        public static bool IsVectorInfinite(Vector2 vec)
        {
            return float.IsInfinity(vec.X) || float.IsInfinity(vec.Y);
        }

        public static Vector2 Abs(Vector2 vec)
        {
            return new Vector2(Math.Abs(vec.X), Math.Abs(vec.Y));
        }

        public static Vector2 Round(Vector2 vec, int precision = 4)
        {
            var p = Math.Pow(10, precision);
            var x = Math.Round(vec.X * p) / p;
            var y = Math.Round(vec.Y * p) / p;
            return new Vector2((float)x, (float)y);
        }

        public static RectangleF ApplyTo(RectangleF rect, Vector2 vec)
        {
            return new RectangleF(rect.X + vec.X, rect.Y + vec.Y, rect.Width, rect.Height);
        }

        internal static Vector2[] Corners(RectangleF rect)
        {
            var topLeft = new Vector2(rect.Left, rect.Top);
            var topRight = new Vector2(rect.Right, rect.Top);
            var bottomLeft = new Vector2(rect.Left, rect.Bottom);
            var bottomRight = new Vector2(rect.Right, rect.Bottom);
            return new Vector2[] { topLeft, bottomLeft, topRight, bottomRight };
        }

        /// <summary>
        /// Highest power of two for x if x is even, x+1 if x is odd.
        /// Don't ask how or why this function works. It's extremely
        /// useful math wizardry.
        /// </summary>
        /// <returns>The power of two divisor.</returns>
        /// <param name="x">x</param>
        public static int HighestPowerOfTwoDivisor(int x)
        {
            var n = x % 2 == 0 ? x : x + 1;
			return n & -n;
        }

        /// <summary>
        /// Solves the 1st degree system of linear equtions given in the following form:
        /// ax + b = c
        /// </summary>
        /// <returns>the solution for x</returns>
        public static double LinSolve(double a, double b, double c)
        {
            if (IsZero(a))
            {
                return AreEqual(b, c) ? double.PositiveInfinity : double.NaN;
            }

            return (c - b) / a;
        }

        /// <summary>
        /// Solves the 2nd degree system of linear equations given in the following form:
        /// ax + by = c
        /// and
        /// wx + uy = v
        /// </summary>
        /// <returns>The solution [x,y] as a tuple, or null if no solution exists.</returns>
        public static Tuple<double, double> LinSolve(double a, double b, double c, double w, double u, double v)
        {
            var echelonMatrix = new Mat3x2(a, b, c, w, u, v);

            // try row swap if first pivot is zero
            if (IsZero(echelonMatrix[0, 0]))
            {
                echelonMatrix.RowSwap();
            }

            // perform standard row operations if first pivot is not zero
            if (!IsZero(echelonMatrix[0, 0]))
            {
                echelonMatrix.RowMultiply(0, 1 / echelonMatrix[0,0]);
                echelonMatrix.RowAdd(0, 1, -echelonMatrix[1, 0]);
            }

            // check for inconsistency on second row
            if (echelonMatrix.IsRowInconsistent(1))
            {
                return null;
            }

            // normalize second row if and only if the second pivot is non-zero
            if (!IsZero(echelonMatrix[1, 1]))
            {
                echelonMatrix.RowMultiply(1, 1 / echelonMatrix[1, 1]);
            }

            // perform last row operation to zero out (0, 1)
            echelonMatrix.RowAdd(1, 0, -echelonMatrix[0, 1]);

            // check for inconsistency on first row
            if (echelonMatrix.IsRowInconsistent(0))
            {
                return null;
            }

            // check first for infinite solutions if rows are all zero, otherwise return solution
            var s1 = double.PositiveInfinity;
            var s2 = double.PositiveInfinity;

            if (!echelonMatrix.IsRowUniform(0))
            {
                s1 = echelonMatrix[0, 2];
            }

            if (!echelonMatrix.IsRowUniform(1))
            {
                s2 = echelonMatrix[1, 2];
            }

            return new Tuple<double, double>(s1, s2);

        }

        private struct Mat3x2
        {
            private readonly double[][] matrix;

            internal Mat3x2(double m00, double m01, double m02, double m10, double m11, double m12)
            {
                this.matrix = new double[2][];
                this.matrix[0] = new[] { m00, m01, m02 };
                this.matrix[1] = new[] { m10, m11, m12 };
            }

            internal double this[int r, int c]
            {
                get
                {
                    return this.matrix[r][c];
                }

                set
                {
                    this.matrix[r][c] = value;
                }
            }

            internal void RowMultiply(int row, double multiplier)
            {
                for (var i = 0; i < 3; i++)
                {
                    this.matrix[row][i] *= multiplier;
                }
            }

            /// <summary>
            /// Adds row1Mult*row1 to row2.
            /// </summary>
            internal void RowAdd(int row1, int row2, double row1Mult = 1.0)
            {
                for (var i = 0; i < 3; i++)
                {
                    this.matrix[row2][i] += this.matrix[row1][i] * row1Mult;
                }
            }

            internal void RowSwap()
            {
                var tmp = this.matrix[0];
                this.matrix[0] = this.matrix[1];
                this.matrix[1] = tmp;
            }

            internal void Zero()
            {
                this.matrix.Initialize();
            }

            internal bool IsRowUniform(int row)
            {
                var x0 = this.matrix[row][0];
                return this.matrix[row].All(x => AreEqual(x, x0));
            }

            internal bool IsRowInconsistent(int row)
            {
                return IsZero(this.matrix[row][0]) && IsZero(this.matrix[row][1]) && !IsZero(this.matrix[row][2]);
            }
        }
    }
}

