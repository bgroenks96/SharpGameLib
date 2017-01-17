using System;
using SharpGameLib.Interfaces;

namespace SharpGameLib.States.Interfaces
{
    public interface IState : IUpdatable, IEquatable<IState>
    {
        uint Id { get; }

        IState Next();

        void OnEnter(IState previousState);

        void OnExit(IState nextState);
    }
}

