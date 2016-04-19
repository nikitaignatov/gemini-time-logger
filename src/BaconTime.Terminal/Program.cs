using System;
using System.Configuration;
using BaconTime.Terminal.Commands;
using Countersoft.Gemini.Api;

namespace BaconTime.Terminal
{
    class Program
    {
        static void Main(string[] argv)
        {
            var args = new MainArgs(argv, help: true, exit: true);
            try
            {
                foreach (var argument in args.Args)
                {
                    Console.WriteLine("{0} = {1}", argument.Key, argument.Value);
                }
                var svc = LoadService();
                if (args.CmdLog) new LogTimeCommand(svc).Execute(args);
                if (args.CmdShowLoggedHours) new ShowLoggedHoursCommand(svc).Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private static ServiceManager LoadService()
        {
            var settings = ConfigurationManager.AppSettings;
            return new ServiceManager(settings["endpoint"], settings["username"], "", settings["apikey"]);
        }
    }
}
