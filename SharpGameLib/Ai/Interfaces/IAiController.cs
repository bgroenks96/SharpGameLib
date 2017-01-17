using System;
using SharpGameLib.Interfaces;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Ai.Interfaces
{
	public interface IAiController : IUpdatable
	{
	}

	public interface IAiController<TEntity> : IAiController where TEntity : IEntity
	{
		TEntity Entity { get; }
	}
}

