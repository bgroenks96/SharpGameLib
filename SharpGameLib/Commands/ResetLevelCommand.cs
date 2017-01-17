using System;
using SharpGameLib.Interfaces;
using SharpGameLib.Commands.Interfaces;
using SharpGameLib.Input;

namespace SharpGameLib.Commands
{
    public class ResetLevelCommand : ICommand
    {
        private readonly IResetLevelCommandReceiver receiver;

        public ResetLevelCommand(IResetLevelCommandReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Execute(params object[] args)
        {
            var inputState = (InputState)args[0];
            if (inputState == InputState.Release)
            {
                this.receiver.ResetLevel();
            }
        }
    }
}

