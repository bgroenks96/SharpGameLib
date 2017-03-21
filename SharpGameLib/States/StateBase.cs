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
using SharpGameLib.States.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.States
{
    public abstract class StateBase : IState
    {
        private TimeSpan timeUntilStateChange = TimeSpan.MaxValue;

        protected StateBase(uint id)
        {
            this.Id = id;
            this.NextState = this;
        }

        public uint Id { get; }

		protected bool IsTimerRunning { get; private set; }

        protected IState NextState { get; set; }

		protected IState TimerState { get; set; }

        protected bool TimeExpired
        {
            get
            {
                return timeUntilStateChange > TimeSpan.Zero;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.IsTimerRunning)
            {
                this.timeUntilStateChange -= gameTime.ElapsedGameTime;
            }
        }

        public virtual IState Next()
        {
			return this.TimeExpired ? this.TimerState : this.NextState;
        }

        public virtual void OnEnter(IState previousState)
        {
            this.NextState = this;
            this.timeUntilStateChange = TimeSpan.MaxValue;
            this.IsTimerRunning = false;
        }

        public virtual void OnExit(IState nextState)
        {
            this.NextState = this;
            this.timeUntilStateChange = TimeSpan.MaxValue;
            this.IsTimerRunning = false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IState);
        }

        public bool Equals(IState state)
        {
            return this.Id == state?.Id;
        }

        public override int GetHashCode()
        {
            return (int)this.Id;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }

		protected void StartStateTimer(TimeSpan duration)
		{
            this.timeUntilStateChange = duration;
			this.IsTimerRunning = true;
		}

		private void OnTimerExpired(object sender, EventArgs e)
		{
			if (this.TimerState == null)
			{
				throw new Exception("Target timer state should not be null!");
			}

			this.IsTimerRunning = false;
		}
    }
}

