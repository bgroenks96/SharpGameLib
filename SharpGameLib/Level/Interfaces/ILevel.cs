using SharpGameLib.Entities.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Level.Interfaces
{
    public interface ILevel
    {
        string Name { get; set; }

        string BackgroundTextureName { get; set; }

        IEntityData[] Entities { get; set; }
    }
}
