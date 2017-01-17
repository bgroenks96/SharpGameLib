using System;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Entities
{
	public sealed class NullEntityData : EntityDataBase
	{
		public override IEntityData Clone()
		{
			return new NullEntityData
			{
				Position = this.Position,
				Type = this.Type,
				Properties = this.Properties
			};
		}
	}
}

