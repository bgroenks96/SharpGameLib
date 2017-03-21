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
using SharpGameLib.Entities.Interfaces;
using System.Collections.Generic;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Entities.Interfaces
{
	public interface IEntityManager
	{
		IStage Stage { get; }

		void RemoveAll();
	}

	public interface IEntityManager<TEntity> : IEntityManager, IEnumerable<TEntity> where TEntity : class, IEntity
	{
		void Add(params TEntity[] entities);

		void Add(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);

		TEntity Nearest(IEntity target, Vector2 minDist = default(Vector2), Vector2 maxDist = default(Vector2));

		TEntity NearestBelow(IEntity target, Vector2 minDist = default(Vector2), Vector2 maxDist = default(Vector2));
	}
}

