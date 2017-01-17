using SharpGameLib.Interfaces;

namespace SharpGameLib.States.Interfaces
{
    public interface IStateMachine<TState> : IUpdatable where TState : class, IState
    {
        TState CurrentState { set; get; }
    }
}
