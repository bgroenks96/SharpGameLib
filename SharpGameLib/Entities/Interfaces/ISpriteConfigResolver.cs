using System;
using SharpGameLib.States.Interfaces;

namespace SharpGameLib.Entities.Interfaces
{
    public interface ISpriteConfigResolver
    {
        SpriteConfig Resolve(params IState[] states);
    }
}

