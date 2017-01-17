using System;
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

