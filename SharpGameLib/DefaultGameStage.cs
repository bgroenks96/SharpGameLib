using System.Collections.Generic;
using System.Linq;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;
using SharpGameLib.Collision.Interfaces;
using SharpGameLib.Graphics.Interfaces;
using SharpGameLib.Commands.Interfaces;
using System;

namespace SharpGameLib
{
    public class DefaultStage : IStage, IToggleCollisionOverlayCommandReceiver
    {
        private readonly ISet<IDrawable> overlayDrawables = new HashSet<IDrawable>();

        private readonly ISet<IStageActor> actors = new HashSet<IStageActor>();

        private readonly ISet<IStageActor> actorAddQueue = new HashSet<IStageActor>();

        private readonly ISet<IStageActor> actorRemoveQueue = new HashSet<IStageActor>();

        private bool isCollisionOverlayEnabled = false;

        public DefaultStage(ICollisionContainer container)
        {
            this.CollisionContainer = container;
        }

		public event EventHandler<StageActorEventArgs> ActorAdded;

		public event EventHandler<StageActorEventArgs> ActorRemoved;

        private IEnumerable<IUpdatable> Updatables { get; set; } = new List<IUpdatable>();

        private IEnumerable<IDrawable> OrderedDrawables { get; set; } = new List<IDrawable>();

        public int DrawPriority { get; set; } = 1;

        public ICollisionContainer CollisionContainer { get; }

        public void AddOverlay(IDrawable drawable)
        {
            this.overlayDrawables.Add(drawable);
            this.SetDrawables();
        }

        public void Add(IStageActor actor)
        {
            this.actorAddQueue.Add(actor);
            actor.OnEnter(this);
        }

        public void RemoveOverlay(IDrawable drawable)
        {
            this.overlayDrawables.Remove(drawable);
            this.SetDrawables();
        }

        public void Remove(IStageActor actor)
        {
            this.actorRemoveQueue.Add(actor);
            actor.OnExit(this);
        }

        public void ClearActors()
        {
            foreach (var actor in this.actors.ToList())
            {
                actor.OnExit(this);
            }

            this.actors.Clear();
            this.CollisionContainer.Clear();
            this.SetDrawables();
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessingPendingActors();

            this.CollisionContainer.Update(gameTime);

            foreach (var actor in this.Updatables)
            {
                actor.Update(gameTime);
            }
        }

        public void Draw(ICanvas canvas)
        {
            foreach (var drawable in this.OrderedDrawables)
            {
                drawable.Draw(canvas);
            }
        }

        public void ToggleBounds()
        {
            this.isCollisionOverlayEnabled = !this.isCollisionOverlayEnabled;
            if (this.isCollisionOverlayEnabled)
            {
                this.AddOverlay(this.CollisionContainer);
            }
            else
            {
                this.RemoveOverlay(this.CollisionContainer);
            }
        }

        private void ProcessingPendingActors()
        {
            foreach (var actor in this.actorRemoveQueue)
            {
                var collidable = actor as ICollidable;
                if (collidable != null)
                {
                    this.CollisionContainer.Remove(collidable);
                }

                this.actors.Remove(actor);
				this.ActorRemoved?.Invoke(this, new StageActorEventArgs(actor));
            }

			foreach (var actor in this.actorAddQueue)
			{
				var collidable = actor as ICollidable;
				if (collidable != null)
				{
					this.CollisionContainer.Add(collidable);
				}

				this.actors.Add(actor);
				this.ActorAdded?.Invoke(this, new StageActorEventArgs(actor));
			}

            this.SetUpdatables();
            this.SetDrawables();
            this.actorAddQueue.Clear();
            this.actorRemoveQueue.Clear();
        }

        private void SetUpdatables()
        {
            this.Updatables = this.actors.OfType<IUpdatable>().ToList();
        }

        private void SetDrawables()
        {
            this.OrderedDrawables = this.actors.OfType<IDrawable>().Concat(this.overlayDrawables).OrderBy(d => d.DrawPriority).ToList();
        }
    }
}

