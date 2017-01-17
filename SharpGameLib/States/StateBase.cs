using System;
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

