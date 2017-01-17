using System;
using SharpGameLib.Interfaces;
using SharpGameLib.Commands.Interfaces;
using SharpGameLib.Input;

namespace SharpGameLib.Commands
{
    public class ToggleCollisionOverlayCommand : ICommand
    {
        private readonly IToggleCollisionOverlayCommandReceiver receiver;

        public ToggleCollisionOverlayCommand(IToggleCollisionOverlayCommandReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Execute(params object[] args)
        {
            var inputState = (InputState)args[0];
            if (inputState == InputState.Release)
            {
                this.receiver.ToggleBounds();
            }
        }
    }
}

