using System;

namespace SharpGameLib.Interfaces
{
    public interface IStageActor
    {
        void OnEnter(IStage stage);

        void OnExit(IStage stage);
        //void OnExit(DefaultBackgroundStage defaultBackgroundStage);
    }
}

