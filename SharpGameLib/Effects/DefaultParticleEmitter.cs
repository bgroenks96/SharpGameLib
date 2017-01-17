using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SharpGameLib.Effects.Interfaces;
using SharpGameLib.Interfaces;
using SharpGameLib.Graphics.Interfaces;

using Microsoft.Xna.Framework;

namespace SharpGameLib.Effects
{
    public class DefaultParticleEmitter : IParticleEmitter
    {
        private IDictionary<IParticle, TimeSpan> particles = new Dictionary<IParticle, TimeSpan>();

        public DefaultParticleEmitter(ParticleFactory factory)
        {
            this.Factory = factory;
        }

		public ParticleFactory Factory { get; set; }

        public bool HasParticles
        {
            get
            {
                return this.particles.Any();
            }
        }

        public int DrawPriority { get; set; } = int.MaxValue;

        private IStage CurrentStage { get; set; }

        public Task Emit(int count)
        {
            var newParticles = new List<IParticle>();
            for (var i = 0; i < count; i++)
            {
                var p = this.Factory(i);
                this.particles[p] = p.Duration;
                newParticles.Add(p);
                p.OnEmit(this);
                if (this.CurrentStage != null)
                {
					this.CurrentStage?.Add(p);
                }
            }

            var maxDuration = newParticles.Max(p => p.Duration);
            return Task.Delay(maxDuration);
        }

        public bool Cancel(IParticle particle)
        {
            var chk = this.particles.Remove(particle);
            if (this.CurrentStage != null)
            {
				this.CurrentStage?.Remove(particle);
            }

            return chk;
        }

        public async Task AwaitExpirationAsync()
        {
            if (!this.HasParticles)
            {
                return;
            }

            var maxDuration = this.particles.Values.Max();
            await Task.Delay(maxDuration);
        }

        public void Update(GameTime gameTime)
        {
            var currentParticleList = this.particles.Keys.ToList();
            foreach (var particle in currentParticleList)
            {
                particle.Velocity += particle.Acceleration;
                particle.Position += particle.Velocity;
                var remaining = this.particles[particle].Subtract(gameTime.ElapsedGameTime);
                if (remaining < TimeSpan.Zero)
                {
                    this.particles.Remove(particle);
                    if (this.CurrentStage != null)
                    {
						this.CurrentStage?.Remove(particle);
                    }
                }
                else
                {
                    this.particles[particle] = remaining;
                }
            }
        }

        public void Draw(ICanvas canvas)
        {
            
        }

        public void OnEnter(IStage stage)
        {
            this.CurrentStage = stage;
            foreach (var particle in this.particles.Keys)
            {
                particle.OnEnter(stage);
            }
        }

        public void OnExit(IStage stage)
        {
            this.CurrentStage = null;
            foreach (var particle in this.particles.Keys)
            {
                particle.OnExit(stage);
            }
        }
    }
}

