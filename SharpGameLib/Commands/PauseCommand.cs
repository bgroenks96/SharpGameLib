using System;
using SharpGameLib.Interfaces;
using SharpGameLib.Input;

namespace SharpGameLib.Commands
{
    public class PauseCommand : ICommand
    {
        private readonly IPauseCommandReceiver receiver;

        public PauseCommand(IPauseCommandReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Execute(params object[] args)
        {
            var inputState = (InputState)args[0];
            if (inputState == InputState.Release)
            {
                this.receiver.Pause();
            }
        }
    }
}

