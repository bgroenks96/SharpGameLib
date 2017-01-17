using System;
using Microsoft.Xna.Framework;
using SharpGameLib.Ai.Interfaces;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Ai
{
	public abstract class AiControllerBase<TEntity> : IAiController<TEntity> where TEntity : IEntity
	{
		protected AiControllerBase (TEntity entity)
		{
			this.Entity = entity;
		}

		public TEntity Entity { get; }

		public abstract void Update(GameTime gameTime);
	}
}

