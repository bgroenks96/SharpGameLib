using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace SharpGameLib.Input
{
    public class KeyboardController : InputControllerBase<KeyboardState>
    {
        private static KeyboardController instance;

        private KeyboardController()
        {
            this.Name = "Keyboard";

            instance = this;
        }

        public static KeyboardController GetKeyboard()
        {
            return instance ?? new KeyboardController();
        }

        protected override KeyboardState ProcessCurrentInputState(KeyboardState previousKeyState)
        {
            var currentKeyState = Keyboard.GetState();
            var currentPressedKeys = currentKeyState.GetPressedKeys();
            var previousPressedKeys = previousKeyState.GetPressedKeys();

            var newlyPressedKeys = new HashSet<Keys>(currentPressedKeys);
            newlyPressedKeys.ExceptWith(previousPressedKeys);
            foreach (var pressedKey in newlyPressedKeys)
            {
                base.FireCommandIfPresent(pressedKey, InputState.Press);
            }

            var holdKeys = currentPressedKeys.Intersect(previousPressedKeys);
            foreach (var holdKey in holdKeys) {
                base.FireCommandIfPresent(holdKey, InputState.Hold);
            }

            var releasedKeys = new HashSet<Keys>(previousPressedKeys);
            releasedKeys.ExceptWith(currentPressedKeys);
            foreach (var releasedKey in releasedKeys)
            {
                base.FireCommandIfPresent(releasedKey, InputState.Release);
            }
            
            return currentKeyState;
        }
    }
}

