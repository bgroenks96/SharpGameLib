﻿using System;
using SharpGameLib.Interfaces;
using SharpGameLib.Commands.Interfaces;
using SharpGameLib.Input;

namespace SharpGameLib.Commands.Gui
{
    public class SelectCommand : ICommand
    {
        private readonly IGuiCommandReceiver receiver;

        public SelectCommand(IGuiCommandReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Execute(params object[] args)
        {
            var inputState = (InputState)args[0];
            if (inputState == InputState.Release)
            {
                this.receiver.Select();
            }
        }
    }
}
