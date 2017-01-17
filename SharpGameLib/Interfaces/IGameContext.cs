using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SharpGameLib.Commands.Interfaces;

namespace SharpGameLib.Interfaces
{
    public interface IGameContext : IQuitCommandReceiver
    {
        GraphicsDevice Graphics { get; }

        IEnumerable<IController> Controllers { get; }

        T LoadContent<T>(string assetName);

        void ConfigureController<TController>(IControllerConfig config) where TController : IController;

        void ResetControllers();
    }
}

