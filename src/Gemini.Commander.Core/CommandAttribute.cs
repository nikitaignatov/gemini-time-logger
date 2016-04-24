using System;

namespace Gemini.Commander.Core
{
    public class CommandAttribute : Attribute
    {
        public string[] Command { get; }

        public string Usage { get; set; }

        public CommandAttribute(params string[] command)
        {
            Command = command;
        }
    }
}