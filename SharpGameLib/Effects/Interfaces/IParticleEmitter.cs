using SharpGameLib.Interfaces;
using System.Collections.Generic;
using SharpGameLib.Sprites.Interfaces;
using System.Threading.Tasks;

namespace SharpGameLib.Effects.Interfaces
{
    public delegate IParticle ParticleFactory(int n);

    public interface IParticleEmitter : IUpdatable, IDrawable, IStageActor
    {
		ParticleFactory Factory { get; set; }

        bool HasParticles { get; }

        /// <summary>
        /// Emits the specified number of particles. The task returned will
        /// be completed when all newly emitted particles have exhausted their
        /// duration.
        /// </summary>
        /// <param name="count">Count.</param>
        Task Emit(int count);

        /// <summary>
        /// Removes the given particle from the current collection, if it exists.
        /// </summary>
        /// <returns><c>true</c> if the particle existed in the emitter and was canceled; otherwise, <c>false</c>.</returns>
        /// <param name="particle">Particle.</param>
        bool Cancel(IParticle particle);

        Task AwaitExpirationAsync();
    }
}

