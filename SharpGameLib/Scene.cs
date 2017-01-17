using System.Linq;
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

