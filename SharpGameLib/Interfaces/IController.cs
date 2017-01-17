using System;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Interfaces
{
    public interface IController
    {
        string Name { get; }

        IControllerConfig Config { get; set; }

        void Register(ICommand command);

        void Unregister(ICommand command);

        void UnregisterAll();

        void Update(GameTime gameTime);
    }
}

