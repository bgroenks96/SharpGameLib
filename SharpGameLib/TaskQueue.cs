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
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SharpGameLib
{
    public static class TaskQueue
    {
		private readonly static Queue<Action> taskQueue = new Queue<Action>();
        private readonly static object mutex = new object();

        public static void Enqueue(Action action)
        {
			EnqueueOffCurrentThread (action);
        }

        public static void ExecuteSynchronously()
        {
			while (taskQueue.Count > 0)
            {
				var next = taskQueue.Dequeue ();
                next.Invoke();
            }
        }

        public static async Task ExecuteAsync()
        {
            var tasks = new List<Task>();
            while (taskQueue.Count > 0)
            {
				var next = taskQueue.Dequeue ();
                tasks.Add(Task.Run(next));
            }

            await Task.WhenAll(tasks);
        }

        private static void EnqueueOffCurrentThread(Action action)
        {
            Task.Run(() =>
                {
                    lock (mutex)
                    {
                        taskQueue.Enqueue(action);
                    }
                });
        }
    }
}

