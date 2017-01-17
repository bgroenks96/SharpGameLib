using System;
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

