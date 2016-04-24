using System;
using System.Linq;
using Gemini.Commander.Core;

namespace Gemini.Commander
{
    using Format = ConsoleTables.Core.Format;

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
