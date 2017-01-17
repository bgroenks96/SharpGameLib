using System;

namespace SharpGameLib.Commands.Interfaces
{
    public interface IGuiCommandReceiver
    {
        void Up();

        void Down();

        void Left();

        void Right();

        void Select();
    }
}

