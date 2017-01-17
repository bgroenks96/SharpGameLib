using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Entities.Interfaces
{
    public interface IEntityData
    {
        string Type { get; set; }

        float[] Repeat { get; set; }

        Vector2 Position { get; set; }

        IDictionary<string, object> Properties { get; set; }

        IEntityData Clone();
    }
}
