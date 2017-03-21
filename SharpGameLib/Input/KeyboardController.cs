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

