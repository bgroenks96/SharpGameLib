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

using SharpGameLib.Interfaces;

using Microsoft.Xna.Framework;
using SharpGameLib.Config;

namespace SharpGameLib.Input
{
    public abstract class InputControllerBase<TInputState> : IController
    {
        private readonly IList<ICommand> commands = new List<ICommand>();

        private TInputState previousInputState;

        protected InputControllerBase(string name = "")
        {
            this.Name = name;
        }

        public IControllerConfig Config { get; set; } = new NullControllerConfig();

        public string Name { get; protected set; }

        public void Register(ICommand command)
        {
            this.commands.Add(command);
        }

        public void Unregister(ICommand command)
        {
            this.commands.Remove(command);
        }

        public void UnregisterAll()
        {
            this.commands.Clear();
        }

        public void Update(GameTime gameTime)
        {
            var currentInputState = this.ProcessCurrentInputState(this.previousInputState);
            this.previousInputState = currentInputState;
        }

        protected abstract TInputState ProcessCurrentInputState(TInputState previousState);

        protected void FireCommandIfPresent(object mapping, params object[] commandArgs)
        {
            if (!this.Config.HasMapping(mapping))
            {
                return;
            }

            var mappedCommand = this.Config.CommandTypeFor(mapping);
			foreach (var command in this.commands.Where(cmd => cmd.GetType().Equals(mappedCommand)).ToList())
            {
                command.Execute(commandArgs);
            }
        }
    }
}

