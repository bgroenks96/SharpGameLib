using System;
using SharpGameLib.Entities.Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Entities
{
	public abstract class EntityDataBase : IEntityData
	{
		public float[] Repeat { get; set; } = new[] { 0f, 0f, 0f };

		public string Type { get; set; } = string.Empty;

		public Vector2 Position { get; set; } = Vector2.Zero;

		public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

		public abstract IEntityData Clone();
	}
}

