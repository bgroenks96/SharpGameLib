using System;
using SharpGameLib.Interfaces;
using SharpGameLib.States.Interfaces;

using SharpGameLib.Sprites.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.States
{
    public abstract class StateMachineBase<TState> : IStateMachine<TState> where TState : class, IState
    {
        private TState currentState;

        public TState CurrentState
        {
            get
            {
                return this.currentState;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                var current = this.currentState;
                var next = value;
                current?.OnExit(next);
                this.currentState = next;
                next.OnEnter(current);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            var current = this.CurrentState;
            if (current == null)
            {
                return;
            }

            current.Update(gameTime);
            var nextState = this.CurrentState.Next() as TState;
            if (nextState == default(TState))
            {
                throw new InvalidOperationException($"illegal state type: {typeof(TState)}");
            }

            this.CurrentState = nextState;
        }
    }
}

