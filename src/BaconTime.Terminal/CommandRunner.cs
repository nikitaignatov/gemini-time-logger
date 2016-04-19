using System;
using System.Collections.Generic;
using System.Configuration;
using BaconTime.Terminal.Commands;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using DocoptNet;
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
            {"log",typeof(LogTimeCommand)},
            {"show",typeof(TimeLoggedForItemCommand)},
            {"show-all",typeof(TimeLoggedByUserGroupedByItemCommand)},
            {"show-days",typeof(ShowLoggedHoursCommand)},
            {"show-words",typeof(FrequentWordsCommand)},
        };

        public void Run(string[] args)
        {
            container = new Container();
            container.Verify();

            ((ICommand)container.GetInstance(Commands[config.command])).Execute(args);
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