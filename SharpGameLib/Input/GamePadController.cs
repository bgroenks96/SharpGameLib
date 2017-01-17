using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

using PlayerIndex = Microsoft.Xna.Framework.PlayerIndex;

namespace SharpGameLib.Input
{
    public class GamePadController : InputControllerBase<GamePadState>
    {
        private const int MaxGamePadCount = 10;
        private static readonly GamePadController[] Controllers = new GamePadController[MaxGamePadCount];

        private readonly GamePadCapabilities capabilities;

        private GamePadController(PlayerIndex index)
        {
            this.capabilities = GamePad.GetCapabilities(index);
            this.PlayerIndex = index;

            this.Name = $"{index}-{this.capabilities.GamePadType.ToString()}";

            Controllers[(int)index] = this;
        }

        public PlayerIndex PlayerIndex { get; }
        
        public static GamePadController GetPrimaryGamePad
        {
            get { return GetGamePadAt(0); }
            /*if (GamePad.MaximumGamePadCount <= 0)
            {
                throw new Exception("No game pads supported on this system.");
            }

            return GetGamePadAt(0); */
        }

        public static GamePadController GetGamePadAt(PlayerIndex index)
        {
            if (MaxGamePadCount <= (int)index)
            {
                throw new Exception("Game pad index out of supported range.");
            }

            return Controllers[(int)index] ?? new GamePadController(index);
        }

        public static bool IsAnyGamePadConnected()
        {
            return IsGamePadConnectedAt(0);
        }

        public static bool IsGamePadConnectedAt(PlayerIndex index)
        {
            return GamePad.GetState(index).IsConnected;
        }

        protected override GamePadState ProcessCurrentInputState (GamePadState previousState)
        {
            var currentState = GamePad.GetState(this.PlayerIndex);
            if (!currentState.IsConnected)
            {
                return currentState;
            }

            foreach (var button in Enum.GetValues(typeof(Buttons)).Cast<Buttons>())
            {
                if (currentState.IsButtonDown(button) && previousState.IsButtonUp(button))
                {
                    base.FireCommandIfPresent(button, InputState.Press);
                }

                if (currentState.IsButtonDown(button) && previousState.IsButtonDown(button))
                {
                    base.FireCommandIfPresent(button, InputState.Hold);
                }

                if (currentState.IsButtonUp(button) && previousState.IsButtonDown(button))
                {
                    base.FireCommandIfPresent(button, InputState.Release);
                }
            }

            return currentState;
        }
    }
}

