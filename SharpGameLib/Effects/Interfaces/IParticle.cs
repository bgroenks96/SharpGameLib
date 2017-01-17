using System;
using SharpGameLib.Interfaces;
using SharpGameLib.Sprites.Interfaces;

namespace SharpGameLib.Effects.Interfaces
{
    public interface IParticle : IMovable, IDrawable, IUpdatable, IStageActor
    {
        ISprite Sprite { get; }

        TimeSpan Duration { get; }

        Guid Id { get; }

        void OnExpired();

        void OnEmit(IParticleEmitter emitter);
    }
}

