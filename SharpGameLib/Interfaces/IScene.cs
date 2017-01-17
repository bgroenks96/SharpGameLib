using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpGameLib.Level.Interfaces;

namespace SharpGameLib.Interfaces
{
    public interface IScene : IUpdatable, IDisposable
    {
        IGameContext Context { get; }

        Rectangle LevelBounds { get; }

        Viewport Viewport { get; }

        ILevel CurrentLevel { get; }

        void Initialize(IGameContext gameContext);

		void Resume(IGameContext gameContext);

        void Draw(GameTime gameTime);
    }
}

