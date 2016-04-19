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
                var svc = LoadService();
                if (args.CmdLog) new LogTimeCommand(svc).Execute(args);
                if (args.CmdShowLoggedHours) new ShowLoggedHoursCommand(svc).Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("\nInput args:\n");
                ConsoleTables.Core.ConsoleTable.From(args.Args.ToList()).Write(Format.MarkDown);
            }
        }

        private static ServiceManager LoadService()
        {
            var settings = ConfigurationManager.AppSettings;
            return new ServiceManager(settings["endpoint"], settings["username"], "", settings["apikey"]);
        }
    }
}
