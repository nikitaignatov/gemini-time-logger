using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using Fclp;
using Fclp.Internals.Extensions;
using Newtonsoft.Json;
using SimpleInjector;

namespace BaconTime.Terminal
{
    class Program
    {
        private static Container container;
        private static ServiceManager svc;
        private static Configuration config;
        private static User user;
        private static IssueTimeTracking log;

        private static readonly Dictionary<string, Type> Commands = new Dictionary<string, Type>
        {
            {"log",typeof(LogHoursCommand)},
        };

        static void Main(string[] args)
        {
            try
            {
                var svc = LoadService(args);
                var user = LoadUser();

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

        private static User LoadUser()
        {
            try
            {
                return svc.Item.WhoAmI().Entity;
            }
            catch (Exception e)
            {
                throw new Exception($"Could not find user with username: {config.username}", e);
            }
        }
    }
}
