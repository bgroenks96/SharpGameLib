using System;
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

