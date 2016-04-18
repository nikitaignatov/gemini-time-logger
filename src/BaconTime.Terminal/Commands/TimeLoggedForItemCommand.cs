using System;
using System.Linq;
using BaconTime.Terminal.Extensions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Fclp;
using Fclp.Internals.Extensions;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    public class TimeLoggedForItemCommand : BaseCommand
    {
        private readonly ServiceManager svc;
        private FluentCommandLineParser p;
        private int issueId;
        private int limit;
        private bool showMyEntriesOnly;

        public TimeLoggedForItemCommand(ServiceManager svc)
        {
            this.svc = svc;
            p = new FluentCommandLineParser();

            p.Setup<int>('t', "ticket").Required().Callback(x => issueId = x).WithDescription("Id of the ticket, for which the log entries shouldbe loaded.");
            p.Setup<int>('l', "limit").SetDefault(10).Callback(x => limit = x).WithDescription("Limit number of entries to be returned");
            p.Setup<bool>("my").SetDefault(false).Callback(x => showMyEntriesOnly = x).WithDescription("Show only my entries.");

            p.SetupHelp("?", "help").Callback(x => Console.WriteLine(x));
        }

        public override void Execute(string[] args)
        {
            ValidateParams(p.Parse(args));

            var issue = svc.Item.Get(issueId);
            var user = svc.Item.WhoAmI();
            var times = svc.Item.GetTimes(issueId);

            Console.WriteLine($"\n\n-> {issue.Title}:\n");

            var table = new ConsoleTable("user", "date", "hours", "message");

            times
                .OrderByDescending(x => x.Entity.EntryDate)
                .Where(x => !showMyEntriesOnly || x.Entity.UserId == user.Entity.Id)
                .Take(limit)
                .Select(x => new
                {
                    user = x.Fullname,
                    date = x.Entity.EntryDate.ToString("yyyy-MM-dd"),
                    Hours = x.Hours(),
                    Message = string.Join("", x.Entity.Comment.Take(25))
                })
                .ForEach(x => table.AddRow(x.user, x.date, x.Hours, x.Message));

            table.Write(Format.MarkDown);
        }
    }
}