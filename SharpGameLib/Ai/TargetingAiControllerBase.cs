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
using SharpGameLib.Entities.Interfaces;
using SharpGameLib.Ai.Interfaces;

namespace SharpGameLib.Ai
{
	public abstract class TargetingAiControllerBase<TEntity> : AiControllerBase<TEntity>, ITargetingAiController<TEntity> where TEntity : IEntity
	{
		protected TargetingAiControllerBase(TEntity entity)
			: base(entity)
		{
		}

		protected float XRange { get; set; } = 5;

		protected float YRange { get; set; } = 5;

		public ITargetableEntity Target { get; set; }

		public Vector2 BaseVelocity { get; set; }

		public override void Update(GameTime gameTime)
		{
			if (this.Target == null)
			{
				return;
			}

			var entityBounds = this.Entity.Bounds;
			var targetBounds = this.Target.Bounds;
			if (entityBounds.Intersects(targetBounds))
			{
				this.OnIntercept ();
				return;
			}

			if ((targetBounds.Left - entityBounds.Left) < -this.XRange)
			{
				this.OnTargetLeft(targetBounds.Left - entityBounds.Left);
			}
			else if ((targetBounds.Left - entityBounds.Left) > this.XRange)
			{
				this.OnTargetRight(targetBounds.Left - entityBounds.Left);
			}
			else
			{
				this.OnTargetInRangeX(targetBounds.Left - entityBounds.Left);
			}

			if (targetBounds.Bottom - entityBounds.Bottom < -this.YRange)
			{
				this.OnTargetAbove(targetBounds.Bottom - entityBounds.Bottom);
			}
			else if (Math.Abs(targetBounds.Bottom - entityBounds.Bottom) > this.YRange)
			{
				this.OnTargetBelow(targetBounds.Bottom - entityBounds.Bottom);
			}
			else
			{
				this.OnTargetInRangeY(targetBounds.Bottom - entityBounds.Bottom);
			}
		}

		protected abstract void OnTargetLeft (float xdist);

		protected abstract void OnTargetRight (float xdist);

		protected abstract void OnTargetAbove (float ydist);

		protected abstract void OnTargetBelow (float ydist);

		protected abstract void OnTargetInRangeX(float xdist);

		protected abstract void OnTargetInRangeY(float ydist);

		protected abstract void OnIntercept ();
	}
}

