using System;

namespace BaconTime.Terminal.Attributes
{
    public class CommandAttribute : Attribute
    {
        public string[] Command { get; }

        public CommandAttribute(params string[] command)
        {
            Command = command;
        }
    }
}