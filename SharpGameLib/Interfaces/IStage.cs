using SharpGameLib.Commands.Interfaces;
using SharpGameLib.Collision.Interfaces;
using System;

namespace SharpGameLib.Interfaces
{
    public interface IStage : IDrawable, IUpdatable
    {
		event EventHandler<StageActorEventArgs> ActorAdded;

		event EventHandler<StageActorEventArgs> ActorRemoved;

        void AddOverlay(IDrawable drawable);

        void Add(IStageActor actor);

        void RemoveOverlay(IDrawable drawable);

        void Remove(IStageActor actor);

        void ClearActors();
    }

	public class StageActorEventArgs : EventArgs
	{
		public StageActorEventArgs(IStageActor actor)
		{
			this.Actor = actor;
		}

		public IStageActor Actor { get; }
	}
}

