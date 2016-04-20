namespace BaconTime.Terminal
{
    using System;
    public class CommandAttribute : Attribute
    {
        public string[] Command { get; }

        public CommandAttribute(params string[] command)
        {
            Command = command;
        }
    }
}