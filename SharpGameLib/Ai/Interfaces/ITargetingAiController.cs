using System;
using SharpGameLib.Entities.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Ai.Interfaces
{
	public interface ITargetingAiController<TEntity> : IAiController<TEntity> where TEntity : IEntity
	{
		ITargetableEntity Target { get; set; }

		Vector2 BaseVelocity { get; set; }
	}
}

