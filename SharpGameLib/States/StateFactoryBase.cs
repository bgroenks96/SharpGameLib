
using SharpGameLib.States.Interfaces;
using System;

namespace SharpGameLib.States
{
    public class StateFactoryBase<TStateBase> : IStateFactory<TStateBase> where TStateBase : IState
	{
        /// <summary>
        /// Fetches a new block state of type TState using simple reflection and a default
        /// constructor parameter pattern.
        /// </summary>
        /// <typeparam name="TBlockState">The 1st type parameter.</typeparam>
        public TStateBase Create<TState>() where TState : TStateBase
        {
            var instance = this.InvokeConstructor<TState>();
            if (instance == null)
            {
                throw new InvalidOperationException(
                    $"Unable to create block state with type {typeof(TState)}; is there a valid constructor?");

            }

            return (TState)instance;
        }

        public TStateBase CreateWithCache<TState>() where TState : TStateBase
        {
            return new AutoCacheStateFactory<TStateBase>(this).Create<TState>();
        }

        protected virtual object InvokeConstructor<T>()
        {
            var constructorParams = new object[] { this };
            return Activator.CreateInstance(typeof(T), constructorParams);
        }
	}
}

