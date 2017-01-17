using SharpGameLib.Level.Interfaces;
using System.IO;

namespace SharpGameLib.Level.Interfaces
{
    public interface ILevelLoader<TLevel> where TLevel : ILevel
    {
        TLevel Load(Stream resourceStream);
    }
}
