using System;

namespace SharpGameLib.Interfaces
{
    public interface IControllerConfig
    {
        Type CommandTypeFor(object mapping);

        bool HasMapping(object mapping);
    }
}

