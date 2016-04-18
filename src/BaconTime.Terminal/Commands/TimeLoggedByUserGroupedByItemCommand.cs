using System;
using System.Linq;
using BaconTime.Terminal.Extensions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;
using Fclp.Internals.Extensions;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    public class TimeLoggedByUserGroupedByItemCommand : BaseCommand
    {
        private readonly ServiceManager svc;
        private FluentCommandLineParser p;
        private int limit;
        private bool showMyEntriesOnly;
        private bool includeClosedTickets;

        public TimeLoggedByUserGroupedByItemCommand(ServiceManager svc)
        {
            this.svc = svc;
            p = new FluentCommandLineParser();

            p.Setup<int>('l', "limit").SetDefault(10).Callback(x => limit = x).WithDescription("Limit the number of enties to be shown.");
            p.Setup<bool>("include-closed").SetDefault(false).Callback(x => includeClosedTickets = x).WithDescription("Include clode tickets");
            p.Setup<bool>("my").SetDefault(false).Callback(x => showMyEntriesOnly = x).WithDescription("Show only my entries.");
            p.SetupHelp("?", "help").Callback(x => Console.WriteLine(x));
        }

        public override void Execute(string[] args)
        {
            ValidateParams(p.Parse(args));

            var user = svc.Item.WhoAmI();
            var items = svc.Item.GetFilteredItems(new IssuesFilter
            {
                IncludeClosed = includeClosedTickets,
                TimeLoggedBy = user.Entity.Id + ""
            }).SelectMany(x => x.TimeEntries.Select(e => new { x.Entity, Time = e })).ToList();

            var times = items.OrderByDescending(x => x.Time.Entity.EntryDate);

            var table = new ConsoleTable("user", "id", "ticket", "date", "hours", "message");

            times
                .Where(x => !showMyEntriesOnly || x.Time.Entity.UserId == user.Entity.Id)
                .Select(x => new object[]
                {
                    x.Time.Fullname.Shorten(10),
                    x.Entity.Id,
                    x.Entity.Title.Shorten(20),
                    x.Time.Entity.EntryDate,
                    x.Time.Hours(),
                    x.Time.Entity.Comment.Shorten(25)
                })
                .Take(limit).
                ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}