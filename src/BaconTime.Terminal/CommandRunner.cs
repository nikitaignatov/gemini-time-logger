using System;
using System.Collections.Generic;
using System.Configuration;
using BaconTime.Terminal.Commands;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;
using Fclp.Internals;
using SimpleInjector;

namespace BaconTime.Terminal
{
    public class CommandRunner
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

        public void Run(string[] args)
        {
            var svc = LoadService(args);
            var user = LoadUser(svc);

            container = new Container();
            container.Register(() => svc);
            container.Register(() => config);
            container.Register(() => user);
            container.Verify();

            ((ICommand)container.GetInstance(Commands[config.command])).Execute(args);
        }

        private ServiceManager LoadService(string[] args)
        {
            var c = new FluentCommandLineParser<Configuration>();
            var settings = ConfigurationManager.AppSettings;
            c.Setup(x => x.endpoint).As('e', "endpoint").SetDefault(settings["endpoint"]).WithDescription("Url to Gemini installation.");
            c.Setup(x => x.username).As('u', "username").SetDefault(settings["username"]).WithDescription("Username for your Gemini account.");
            c.Setup(x => x.key).As('k', "apikey").SetDefault(settings["apikey"]).WithDescription("Api Key for your account, you can find it by opening you account settings > password tab.");
            c.Setup(x => x.command).As("cmd").Required().WithDescription("Name of the command that will be executed");

            c.SetupHelp("?", "help").Callback(x => Console.WriteLine(x));
            var result = c.Parse(args);

            if (result.HasErrors) throw new Exception(result.ErrorText);

            config = c.Object;
            return new ServiceManager(config.endpoint, config.username, "", config.key);
        }

        private User LoadUser(ServiceManager svc)
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