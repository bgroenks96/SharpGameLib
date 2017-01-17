using System;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Entities.Interfaces
{
	public interface ITargetableEntity : IEntity
	{
		event EventHandler Damaged;

		void TakeDamage (int amount);

		void Die();
	}
}
