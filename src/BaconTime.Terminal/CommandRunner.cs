using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Countersoft.Gemini.Api;
using DocoptNet;
using SimpleInjector;

namespace BaconTime.Terminal
{
    public class CommandRunner
    {
        private static Container container;

        public void Run(MainArgs args)
        {
            container = new Container();
            container.Register(LoadService);
            container.Verify();
            var cmd = typeof(ICommand).Assembly.ExportedTypes;
            var command = LoadCommand(args.Args, cmd);
            ((ICommand)container.GetInstance(command)).Execute(args);
        }

        public static Type LoadCommand(IDictionary<string, ValueObject> args, IEnumerable<Type> types)
        {
            var commands = types
                .Where(x => typeof(ICommand).IsAssignableFrom(x))
                .Where(x => !(x.IsAbstract || x.IsInterface))
                .Where(x => x.IsDefined(typeof(CommandAttribute), false))
                .Select(x => new
                {
                    type = x,
                    command = x.GetCustomAttributes(typeof(CommandAttribute), false).OfType<CommandAttribute>().First().Command
                })
                .Where(x => x.command.All(c => args[c].IsTrue))
                .ToArray();

            if (!commands.Any())
            {
                throw new Exception("no command matched the provided args");
            }

            if (commands.Count(x => x.command.All(c => args[c].IsTrue)) > 1)
            {
                var existingCommands = string.Join("\\n", commands.Select(x => x.type.Name));
                throw new Exception($"make sure only one command is matching the args, in this case following commands matched:\\n{existingCommands}");
            }

            var command = commands.First();
            return command.type;
        }

        private static ServiceManager LoadService()
        {
            var settings = ConfigurationManager.AppSettings;
            return new ServiceManager(settings["endpoint"], settings["username"], "", settings["apikey"]);
        }
    }
}