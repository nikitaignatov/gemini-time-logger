namespace Gemini.Commander.Core
{
    public interface ICommand
    {
        void Execute(MainArgs args);
    }

    public interface IQuery<out TResult>
    {
        TResult Execute(MainArgs args);
    }
}