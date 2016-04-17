using System;
using System.Collections.Generic;
using System.Linq;
using BaconTime.Terminal.Commands;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;
using SimpleInjector;

namespace BaconTime.Terminal
{
    class Program
    {
        private static Container container;
        private static Configuration config;
        private static IssueTimeTracking log;

        private static readonly Dictionary<string, Type> Commands = new Dictionary<string, Type>
        {
            {"log",typeof(LogHoursCommand)},
            {"show",typeof(TimeLoggedForItemCommand)},
            {"show-all",typeof(TimeLoggedByUserGroupedByItemCommand)},
        };

        static void Main(string[] args)
        {
            try
            {
                var svc = LoadService(args);
                var user = LoadUser(svc);

                container = new Container();
                container.Register(() => svc);
                container.Register(() => config);
                container.Register(() => user);
                container.Verify();

                var cmd = args.FirstOrDefault();
                args = args.Skip(1).ToArray();
                ((ICommand)container.GetInstance(Commands[cmd])).Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError occured.");
                Console.WriteLine(e.Message);
            }
        }

        private static ServiceManager LoadService(string[] args)
        {
            var c = new FluentCommandLineParser<Configuration>();
            c.Setup(x => x.endpoint).As('e', "endpoint").Required();
            c.Setup(x => x.username).As('u', "username").Required();
            c.Setup(x => x.key).As('k', "apikey").Required();

            var result = c.Parse(args);
            if (result.HasErrors) throw new Exception(result.ErrorText);

            config = c.Object;
            return new ServiceManager(config.endpoint, config.username, "", config.key);
        }

        private static User LoadUser(ServiceManager svc)
        {
            try
            {
                return svc.Item.WhoAmI().Entity;
            }
            catch (Exception e)
            {
                throw new Exception($"Could not find user with username: {config.username}, " + e.Message, e);
            }
        }
    }
}
