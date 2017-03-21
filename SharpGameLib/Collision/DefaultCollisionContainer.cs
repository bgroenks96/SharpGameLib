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
using System.Collections.Generic;
using SharpGameLib.Collision.Interfaces;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Collision
{
    public class DefaultCollisionContainer : ICollisionContainer
    {
        // a constant for determining significance of a velocity vector's magnitude
        private const float VelocityEpsilon = 1E-5f;
        private const float DefaultCellWidth = 30;
        private const float DefaultCellHeight = 30;

        private readonly IList<ICollidable> collidables = new List<ICollidable>();
        private readonly IDictionary<ulong, Cell> cells = new Dictionary<ulong, Cell>();
		private readonly IDictionary<ICollidable, IEnumerable<Cell>> memberCellCache = new Dictionary<ICollidable, IEnumerable<Cell>>();

        private readonly int gridWidth;
        private readonly int gridHeight;
        private readonly float cellWidth;
        private readonly float cellHeight;

        public DefaultCollisionContainer(Rectangle containerBounds)
        {
            this.ContainerBounds = containerBounds;

            // var width = containerBounds.Width;
            // var height = containerBounds.Height;
            this.cellWidth = DefaultCellWidth;
            this.cellHeight = DefaultCellHeight;
            this.gridWidth = (int)Math.Ceiling(containerBounds.Width / this.cellWidth);
            this.gridHeight = (int)Math.Ceiling(containerBounds.Height / this.cellHeight);

            this.InitializeCells();
        }

        public event CollisionEventHandler Collision;

        public event CompleteEventHandler ProcessingComplete;

        public int DrawPriority { get; set; } = int.MaxValue;

        public Rectangle ContainerBounds { get; }

        public IEnumerable<ICollidable> Collidables
        {
            get
            {
                return this.collidables;
            }
        }

        public void Add(ICollidable collidable)
        {
            this.collidables.Add(collidable);
            this.AssignCells(collidable);
        }

        public void Remove(ICollidable collidable)
        {
            this.collidables.Remove(collidable);
            this.ClearCells(collidable);
        }

        public void Clear()
        {
            this.collidables.Clear();
            this.ClearCells();
        }

        public void Update(GameTime gameTime)
        {
            var processedCollidableCount = 0;
            var cellHitCount = 0;

            var stopwatch = new Stopwatch();
            foreach (var collidable in this.collidables.Where(c => c.IsEnabled))
            {
                // skip non-moving collidables
                if (GetEffectiveVelocity(collidable).Length() < VelocityEpsilon)
                {
                    continue;
                }

                // update cells
				this.ClearCells(collidable);
                var memberCells = this.AssignCells(collidable);

                cellHitCount += memberCells.Count;

                var collisionCandidates = this.ScanNearbyCells(collidable, memberCells)
                    .OrderBy(cand => Vector2.Distance(cand.Position, collidable.Position)).ToList();

                processedCollidableCount += collisionCandidates.Count;
                this.CheckForCollisions(collidable, collisionCandidates);
            }
			stopwatch.Stop();

            this.ProcessingComplete?.Invoke(
                this,
                new ProcessingCompleteEventArgs(
                    stopwatch.Elapsed,
                    processedCollidableCount,
                    cellHitCount,
                    this.cells.Count));
        }

        public void Draw(ICanvas canvas)
        {
            for (var x = 0; x < this.gridWidth; x++)
            {
                for (var y = 0; y < this.gridHeight; y++)
                {
                    var xpos = (int)(x * this.cellWidth);
                    var ypos = (int)(y * this.cellHeight);
                    var wt = (int)this.cellWidth;
                    var ht = (int)this.cellHeight;
                    var rect = new Rectangle(xpos, ypos, wt, ht);
                    if (!Scene.Viewport.Bounds.Intersects(rect))
                    {
                        continue;
                    }

                    var cell = this.cells[CellHashKey(new Vector2(xpos, ypos))];

                    canvas.DrawRect(rect, cell.IsEmpty() ? Color.Black : Color.LightGreen, 0.75f);
                }
            }

            foreach (var collidable in this.cells.Values.SelectMany(cell => cell.GetMembers()))
            {
                var rect = collidable.Bounds.ToIntegerRect();
                if (!Scene.Viewport.Bounds.Intersects(rect))
                {
                    continue;
                }

                canvas.DrawRect(rect, collidable.DebugColor, 0);
            }
        }

        private IEnumerable<ICollidable> ScanNearbyCells(ICollidable collidable, ISet<Cell> memberCells)
        {
            var neighbors = memberCells.SelectMany(cell => this.NearestNeighbors(cell)).Where(cell => !cell.IsEmpty());
            return neighbors.SelectMany(cell => cell.GetMembersExcept(collidable)).Distinct().Where(c => c.IsEnabled);
        }

        private void CheckForCollisions(ICollidable collidable, IEnumerable<ICollidable> candidates)
        {
            var vel = GetEffectiveVelocity(collidable);
            var velX = new Vector2(vel.X, 0);
            var velY = new Vector2(0, vel.Y);
            var modelX = new CollisionModel(collidable.Bounds, velX);
            var modelY = new CollisionModel(collidable.Bounds, velY);
            foreach (var other in candidates)
            {
                var otherVel = GetEffectiveVelocity(other);

//                var otherModel = new CollisionModel(other.Bounds, otherVel);
//                var collision = model.GetCollision(otherModel);
//                if (!collision.IsNullCollision())
//                {
//                    collidable.OnCollideWith(other, collision.ReversePolarity(), this);
//                    other.OnCollideWith(collidable, collision, this);
//                    this.Collision?.Invoke(this, new CollisionEventArgs(collidable, other, collision));
//                }

                var otherVelX = new Vector2(otherVel.X, 0);
                var otherVelY = new Vector2(0, otherVel.Y);
                var otherModelX = new CollisionModel(other.Bounds, otherVelX);
                var otherModelY = new CollisionModel(other.Bounds, otherVelY);
                var collisionX = modelX.GetCollision(otherModelX);
                var collisionY = modelY.GetCollision(otherModelY);
                if (!collisionY.IsNullCollision())
                {
                    collidable.OnCollideWith(other, collisionY.ReversePolarity(), this);
                    other.OnCollideWith(collidable, collisionY, this);
                    this.Collision?.Invoke(this, new CollisionEventArgs(collidable, other, collisionY));
                }

                if (!collisionX.IsNullCollision())
                {
                    collidable.OnCollideWith(other, collisionX.ReversePolarity(), this);
                    other.OnCollideWith(collidable, collisionX, this);
                    this.Collision?.Invoke(this, new CollisionEventArgs(collidable, other, collisionX));
                }
            }

            var bounds = RectangleF.From(this.ContainerBounds);
            if (!modelX.IsInside(bounds) || !modelY.IsInside(bounds))
            {
                var pseg = new LineSegment(collidable.Position, vel);
                var intersection = bounds.Intersection(pseg);
                collidable.OnContainerExit(intersection, this);
            }
        }

        private ISet<Cell> AssignCells(ICollidable collidable)
        {
			var targetBounds = MathUtils.ApplyTo(collidable.Bounds, GetEffectiveVelocity(collidable));
			var memberCells = this.CellsFor(RectangleF.Union(collidable.Bounds, targetBounds));
            foreach (var cell in memberCells)
            {
                cell.Add(collidable);
            }

			this.memberCellCache.Remove(collidable);
			this.memberCellCache[collidable] = memberCells;
            return memberCells;
        }

        private void ClearCells(ICollidable collidable)
        {
			if (!memberCellCache.ContainsKey(collidable))
			{
				return;
			}

			foreach (var cell in this.memberCellCache[collidable])
            {
                cell.Remove(collidable);
            }
        }

        private void ClearCells()
        {
            foreach (var cell in this.cells.Values)
            {
                cell.Clear();
            }
        }

        private ISet<Cell> CellsFor(RectangleF rect)
        {
            var memberCells = new HashSet<Cell>();
            var left = Math.Max(rect.Left, this.ContainerBounds.Left);
            var right = Math.Min(rect.Right, this.ContainerBounds.Right);
            var top = Math.Max(rect.Top, this.ContainerBounds.Top);
            var bottom = Math.Min(rect.Bottom, this.ContainerBounds.Bottom);
			for (var x = left; x <= right; x += MathUtils.SelectMid(this.cellWidth, right - x - 1, 1))
            {
				for (var y = top; y <= bottom; y += MathUtils.SelectMid(this.cellHeight, bottom - y - 1, 1))
                {
                    var cellHashKey = CellHashKey(new Vector2(x, y));
					if (!cells.ContainsKey (cellHashKey)) 
					{
						continue;
					}

                    memberCells.Add(this.cells[cellHashKey]);
                }
            }

            return memberCells;
        }

        private void InitializeCells()
        {
            for (var x = 0; x < this.gridWidth; x++)
            {
                for (var y = 0; y < this.gridHeight; y++)
                {
                    var cellHashKey = CellHashKey(x, y);
                    this.cells[cellHashKey] = new Cell(x, y);
                }
            }
        }

        /// <summary>
        /// Returns the nearest neighbors of the given cell. The returned set
        /// includes the given cell in it.
        /// </summary>
        /// <returns>The neighbors.</returns>
        /// <param name="cell">Cell.</param>
        private ISet<Cell> NearestNeighbors(Cell cell)
        {
            var neighbors = new HashSet<Cell>();
            for (var x = Math.Max(0, cell.X - 1); x < Math.Min(this.gridWidth, cell.X + 1); x++)
            {
                for (var y = Math.Max(0, cell.Y - 1); y < Math.Min(this.gridHeight, cell.Y + 1); y++)
                {
                    var cellHashKey = CellHashKey(x, y);
                    neighbors.Add(this.cells[cellHashKey]);
                }
            }

            return neighbors;
        }

        private ulong CellHashKey(Vector2 position)
        {
            var cellX = (int)Math.Floor(position.X / this.cellWidth);
            var cellY = (int)Math.Floor(position.Y / this.cellHeight);
            return CellHashKey(cellX, cellY);
        }

        private static ulong CellHashKey(int cellX, int cellY)
        {
            var cx = (ulong)cellX;
            var cy = (ulong)cellY;
            return cx << 32 | cy;
        }

        private static Vector2 GetEffectiveVelocity(ICollidable collidable)
        {
            return collidable.Velocity + collidable.Acceleration;
        }

		private struct Cell
        {
            internal Cell(int x, int y)
            {
                this.X = x;
                this.Y = y;
				this.Members = new HashSet<ICollidable>();
            }

            internal int X { get; }

            internal int Y { get; }

			private ISet<ICollidable> Members { get; }

            internal bool Has(ICollidable collidable)
            {
                return Members.Contains(collidable);
            }

            internal void Add(ICollidable collidable)
            {
                Members.Add(collidable);
            }

            internal void Remove(ICollidable collidable)
            {
                Members.Remove(collidable);
            }

            internal void Clear()
            {
                Members.Clear();
            }

            internal bool IsEmpty()
            {
                return !Members.Any();
            }

            internal IEnumerable<ICollidable> GetMembers()
            {
                return this.Members;
            }

            internal IEnumerable<ICollidable> GetMembersExcept(ICollidable self)
            {
                return this.Members.Where(coll => !coll.Equals(self));
            }

			public override bool Equals(object obj)
			{
				if (!(obj is Cell))
				{
					return false;
				}

				var cell = (Cell)obj;
				return this.X == cell.X && this.Y == cell.Y;
			}

			public override int GetHashCode()
			{
				return (27 * this.X) ^ (271 * this.Y);
			}
        }
    }
}

