using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Countersoft.Gemini.Api;
using DocoptNet;
using Gemini.Commander.Core;
using SimpleInjector;

namespace Gemini.Commander
{
    public class CommandRunner
    {
        private static Container container;

        public void Run(MainArgs args)
        {
            container = new Container();
            container.Register(LoadService);

            var pluginsDir = Path.Combine(Environment.CurrentDirectory, "plugins");

            if (Directory.Exists(pluginsDir))
                Directory.GetFiles(pluginsDir, "*.dll").ToList().Select(Assembly.LoadFrom).ToList();

            Directory.GetFiles(Environment.CurrentDirectory, "*.dll").Select(Assembly.LoadFrom).ToList();

            var cmd = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(TryGetTypes)
                .Where(x => typeof(ServiceManagerCommand).IsAssignableFrom(x))
                .ToArray();

            container.RegisterCollection<ICommand>();
            container.Verify();
            var command = LoadCommand(args.Args, cmd);
            ((ICommand)container.GetInstance(command)).Execute(args);
        }

        private static Type[] TryGetTypes(Assembly x)
        {
            try
            {
                return x.GetTypes();
            }
            catch (Exception)
            {
                return new Type[0];
            }
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
                .Where(x => x.command.All(c => args.ContainsKey(c) && args[c].IsTrue))
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