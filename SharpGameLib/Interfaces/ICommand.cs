using System;

namespace SharpGameLib.Interfaces
{
    public interface ICommand
    {
        void Execute(params object[] args);
    }
}

