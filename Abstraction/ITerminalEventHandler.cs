namespace GameWarriors.TerminalDomain.Abstraction
{
    public interface ITerminalEventHandler
    {
        void OnOneArgInvoke(string methodName, string payload);
        void OnZeroArgInvoke(string methodName);
    }
}
