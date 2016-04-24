using System;
using System.Linq;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using Gemini.Commander.Core;
using Format = ConsoleTables.Core.Format;

namespace Gemini.Commander.Commands
{
    [Command("create", "ticket")]
    public class CreateTicketCommand : ServiceManagerCommand
    {
        public CreateTicketCommand(ServiceManager svc) : base(svc) { }

        public override void Execute(MainArgs args)
        {
            var now = DateTime.Now;
            var issue = Svc.Item.Create(new Issue
            {
                Title = args.Arguments.Title,
                Created = now,
                ProjectId = args.Arguments.Project,
                ParentIssueId = args.Options.Parent
            });

            Console.WriteLine($"Created issue: {issue.Id}");
        }
    }
}