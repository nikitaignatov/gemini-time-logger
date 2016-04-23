using System;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    [Command("show", "project", "all")]
    public class ShowProjectCommand : ServiceManagerCommand
    {
        public ShowProjectCommand(ServiceManager svc) : base(svc) { }

        public override void Execute(MainArgs args)
        {
            var projects = Svc.Projects.GetProjects();
            var take = args.Options.Take;

            var table = new ConsoleTable("id", "project");

            projects
                .Select(x => new object[]
                {
                    x.Entity.Id,
                    x.Entity.Name
                })
                .Take(take).ToList().
                ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}