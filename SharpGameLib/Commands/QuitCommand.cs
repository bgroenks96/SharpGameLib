using System;

using SharpGameLib.Commands.Interfaces;
using SharpGameLib.Interfaces;

namespace SharpGameLib.Commands
{
    public class QuitCommand : ICommand
    {
        private readonly IQuitCommandReceiver receiver;

        public QuitCommand(IQuitCommandReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Execute(params object[] args)
        {
            this.receiver.Quit();
        }
    }
}

