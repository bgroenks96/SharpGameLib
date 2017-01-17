using System;
using SharpGameLib.Interfaces;
using System.Collections.Generic;
using SharpGameLib.States.Interfaces;

namespace SharpGameLib.States
{
    /// <summary>
    /// IStateFactory decorated that automatically caches created instances by type.
    /// </summary>
    public sealed class AutoCacheStateFactory<TStateBase> : IStateFactory<TStateBase> where TStateBase : IState
    {
        private readonly IDictionary<Type, TStateBase> StateTypeCache = new Dictionary<Type, TStateBase>();

        private readonly IStateFactory<TStateBase> factory;

        public AutoCacheStateFactory(IStateFactory<TStateBase> factory)
        {
            this.factory = factory;
        }

        public TStateBase Create<TState>() where TState : TStateBase
        {
            return this.ShouldCreate(typeof(TState)) ? this.factory.Create<TState>() : this.StateTypeCache[typeof(TState)];
        }

        public TStateBase CreateWithCache<TState>() where TState : TStateBase
        {
            return this.Create<TState>();
        }

        private bool ShouldCreate(Type type)
        {
            return !this.StateTypeCache.ContainsKey(type);
        }
    }
}

