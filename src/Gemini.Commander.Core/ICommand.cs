namespace Gemini.Commander.Core
{
    public interface ICommand
    {
        void Execute(MainArgs args);
    }
}