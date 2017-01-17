using System;
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

