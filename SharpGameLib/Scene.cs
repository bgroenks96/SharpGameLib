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

﻿using System.Linq;
using System.Collections.Generic;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace SharpGameLib
{
    public static class Scene
    {
		private static readonly Stack<IScene> SceneStack = new Stack<IScene>();

        public static IScene Current { get; private set; }

        public static IScene Previous { get; private set; }

        public static Viewport Viewport
        {
            get
            {
                return Current?.Viewport ?? default(Viewport);
            }
        }

        public static void SetCurrent(IScene scene, IGameContext context)
        {
			if (SceneStack.Any())
			{
				SceneStack.Pop();
			}

			Push(scene, context);
        }

		public static void Push(IScene scene, IGameContext context)
		{
			context.ResetControllers();
			Previous = Current;
			Current = scene;
			Current.Initialize(context);
			SceneStack.Push(scene);
		}

		public static void Pop(IGameContext context)
		{
			context.ResetControllers();
			Previous = Current;
			SceneStack.Pop();
			Current = SceneStack.Peek();
			Previous?.Dispose();
			Current.Resume(context);
		}
    }
}

