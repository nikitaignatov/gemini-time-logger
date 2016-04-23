using System;
using System.Linq;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    [Command("create", "ticket")]
    public class CreateTicketCommand : ServiceManagerCommand
    {
        public CreateTicketCommand(ServiceManager svc) : base(svc) { }
        public IssueTimeTracking ToIssueTimeTracking(MainArgs args)
        {
            return new IssueTimeTracking
            {
                IssueId = args.Arguments.Id,
                TimeTypeId = args.Options.LogType,
                Hours = args.Arguments.Hours,
                Minutes = args.Arguments.Minutes,
                Comment = args.Arguments.Message,
                EntryDate = args.Options.When
            };
        }

        public override void Execute(MainArgs args)
        {
            var status = Svc.Meta.GetIssuesStatuses();

            var table = new ConsoleTable("id","template", "project");

            status
                .Select(x => new object[]
                {
                    x.Entity.Id,
                    x.Entity.TemplateId,
                    x.Entity.Label
                })
                .ToList()
                .ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
            
        }
    }
}