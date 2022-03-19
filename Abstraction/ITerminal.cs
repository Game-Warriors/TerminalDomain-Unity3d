using System;

namespace GameWarriors.TerminalDomain.Abstraction
{
    public interface ITerminal
    {
        void Register(Action<string> method, string methodName);
        void Register(Action method, string methodName);
    }
}