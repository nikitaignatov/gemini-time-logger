namespace BaconTime.Terminal
{
    public interface ICommand
    {
        void Execute(string[] args);
    }
}