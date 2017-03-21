/*
 * Copyright (C) 2016-2017 (See COPYRIGHT.txt for holders)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

﻿using System;
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

