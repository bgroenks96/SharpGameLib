using System;
using SharpGameLib.Interfaces;
using SharpGameLib.Commands.Interfaces;
using SharpGameLib.Input;

namespace SharpGameLib.Commands.Gui
{
    public class DownCommand : ICommand
    {
        private readonly IGuiCommandReceiver receiver;

        public DownCommand(IGuiCommandReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Execute(params object[] args)
        {
            var inputState = (InputState)args[0];
            if (inputState == InputState.Release)
            {
                this.receiver.Down();
            }
        }
    }
}

