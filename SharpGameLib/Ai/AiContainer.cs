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
using System.Collections.Generic;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections;
using SharpGameLib.Ai.Interfaces;

namespace SharpGameLib.Ai
{
	public class AiContainer : IUpdatable, IEnumerable<IAiController>
	{
		private readonly IList<IAiController> aiControllers = new List<IAiController>();

		public void Add(IAiController controller)
		{
			this.aiControllers.Add(controller);
		}

		public void Remove(IAiController controller)
		{
			this.aiControllers.Remove(controller);
		}

		public void Update(GameTime gameTime)
		{
			foreach (var controller in this.aiControllers)
			{
				controller.Update(gameTime);
			}
		}

		public IEnumerator<IAiController> GetEnumerator()
		{
			return this.aiControllers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.aiControllers.GetEnumerator();
		}
	}
}

