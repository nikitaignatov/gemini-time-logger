using System;
using System.Configuration;
using System.Linq;
using BaconTime.Terminal.Commands;
using Countersoft.Foundation.Commons.Extensions;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;
using SimpleInjector;

namespace BaconTime.Terminal
{
    class Program
    {
        static void Main(string[] argv)
        {
            var args = new MainArgs(argv, exit: true);
            try
            {
                Console.WriteLine(args.Args.ToJson());
                foreach (var argument in args.Args)
                {
                    Console.WriteLine("{0} = {1}", argument.Key, argument.Value);
                }
                var svc = LoadService();
                if (args.CmdLog) new LogHoursCommand(svc).Execute(args);
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
