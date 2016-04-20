using System;
using System.Configuration;
using System.Linq;
using BaconTime.Terminal.Commands;
using Countersoft.Gemini.Api;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal
{
    class Program
    {
        static void Main(string[] argv)
        {
            var args = new MainArgs(argv, help: true, exit: true);
            try
            {
                new CommandRunner().Run(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("\nInput args:\n");
                ConsoleTables.Core.ConsoleTable.From(args.Args.ToList()).Write(Format.MarkDown);
            }
        }
    }
}
