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
using SharpGameLib.Interfaces;
using SharpGameLib.Entities.Interfaces;
using System.Collections;
using Microsoft.Xna.Framework;
using SharpGameLib.Collision;

namespace SharpGameLib.Entities
{
	public abstract class EntityManagerBase<TEntity> : IEntityManager<TEntity> where TEntity : class, IEntity
	{
		private readonly List<TEntity> entities = new List<TEntity>();

		protected EntityManagerBase (IStage stage)
		{
			this.Stage = stage;
			stage.ActorAdded += OnStageAdd;
			stage.ActorRemoved += OnStageRemove;
		}

		protected IEnumerable<TEntity> SortedEntities { get; set; } = new TEntity[0];

		public IStage Stage { get; }

		public void Add(params TEntity[] entities)
		{
			this.Add(entities as IEnumerable<TEntity>);
			this.Sort ();
		}

		public void Add(IEnumerable<TEntity> entities)
		{
			this.entities.AddRange(entities);
			this.Sort ();
		}

		public void Remove(TEntity entity)
		{
			this.entities.Remove(entity);
			this.Sort ();
		}

		public void RemoveAll()
		{
			this.entities.Clear();
			this.Sort ();
		}

		public TEntity Nearest(IEntity target, Vector2 minDist = default(Vector2), Vector2 maxDist = default(Vector2))
		{
			return this.entities.OrderBy(e => Vector2.Distance(target.Position, e.Position))
				.FirstOrDefault(e => InRange(MathUtils.Abs(LowerLeft(target) - LowerLeft(e)), minDist, maxDist));
		}

		public TEntity NearestBelow(IEntity target, Vector2 minDist = default(Vector2), Vector2 maxDist = default(Vector2))
		{
			return this.entities.OrderBy(e => Vector2.Distance(LowerLeft(target), LowerLeft(e)))
				.FirstOrDefault(e => InRange(MathUtils.Abs(LowerLeft(target) - LowerLeft(e)), minDist, maxDist));
		}

		public IEnumerator<TEntity> GetEnumerator()
		{
			return this.entities.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.entities.GetEnumerator ();
		}

		protected virtual void Sort()
		{
			this.SortedEntities = this.entities.OrderBy (e => e.Position, new VectorComparer());
		}

		protected virtual void OnStageAdd (object sender, StageActorEventArgs e)
		{
			var entity = e.Actor as TEntity;
			if (entity == default(TEntity))
			{
				return;
			}

			this.Add(entity);
		}

		protected virtual void OnStageRemove (object sender, StageActorEventArgs e)
		{
			var entity = e.Actor as TEntity;
			if (entity == default(TEntity))
			{
				return;
			}

			this.Remove(entity);
		}

		protected static bool InRange(Vector2 vec, Vector2 minDist = default(Vector2), Vector2 maxDist = default(Vector2))
		{
			return vec.X <= maxDist.X && vec.X >= minDist.X && 
				vec.Y <= maxDist.Y && vec.Y >= minDist.Y;
		}

		private static Vector2 LowerLeft(IEntity e)
		{
			return new Vector2(e.Bounds.Left, e.Bounds.Bottom);
		}

		private class VectorComparer : IComparer<Vector2>
		{
			public int Compare(Vector2 v1, Vector2 v2)
			{
				return v1.LengthSquared().CompareTo(v2.LengthSquared());
			}
		}
	}
}

