using System;
using SharpGameLib.States.Interfaces;

namespace SharpGameLib.States.Interfaces
{
    public interface IStateFactory<TStateBase> where TStateBase : IState
    {
        TStateBase Create<TState>() where TState : TStateBase;

        /// <summary>
        /// Creates a state with the given type that uses a cache for state transitions.
        /// </summary>
        /// <returns>The cached.</returns>
        /// <typeparam name="TBlockState">The 1st type parameter.</typeparam>
        TStateBase CreateWithCache<TState>() where TState : TStateBase;
    }
}

