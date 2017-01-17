using System;
using SharpGameLib.Interfaces;

namespace SharpGameLib.Config
{
    public sealed class NullControllerConfig : IControllerConfig
    {
        public NullControllerConfig()
        {
        }

        Type IControllerConfig.CommandTypeFor(object mapping)
        {
            throw new NotImplementedException();
        }

        public bool HasMapping(object mapping)
        {
            return false;
        }
    }
}

