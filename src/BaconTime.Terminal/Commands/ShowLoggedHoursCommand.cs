using System;
using System.Linq;
using BaconTime.Terminal.Extensions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp.Internals.Extensions;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    [Command("show", "hours", "my")]
    public class ShowLoggedHoursCommand : BaseCommand
    {
        private readonly ServiceManager svc;
        private int take = 100;
        private int workingHours;

        public ShowLoggedHoursCommand(ServiceManager svc)
        {
            this.svc = svc;
        }

        public override void Execute(MainArgs args)
        {
            var user = svc.Item.WhoAmI();

            workingHours = args.Options.WorkingHours;
            take = args.Options.Take;
            var items = svc.Item
                .GetFilteredItems(new IssuesFilter
                {
                    IncludeClosed = true,
                    TimeLoggedBy = user.Entity.Id + "",
                    TimeLoggedAfter = args.OptFrom.ToString("yyyy-MM-dd"),
                    TimeLoggedBefore = args.OptTo.ToString("yyyy-MM-dd")
                })
                .SelectMany(x => x.TimeEntries.Select(e => new { x.Entity, Time = e }))
                .ToList();

            var times = items.OrderByDescending(x => x.Time.Entity.EntryDate);

            var table = new ConsoleTable("date", "hours", "missing hours");

            times
                .Where(x => x.Time.Entity.UserId == user.Entity.Id)
                .Where(x => x.Time.Entity.EntryDate >= args.OptFrom)
                .Where(x => x.Time.Entity.EntryDate <= args.OptTo)
                .GroupBy(x => x.Time.Entity.EntryDate.Date)
                .OrderByDescending(x => x.Key)
                .Select(x => new object[]
                {
                    x.Key.ToString("yyyy-MM-dd"),
                    x.Sum(m=>m.Time.Hours()),
                    workingHours- x.Sum(m=>m.Time.Hours())
                })
                .Take(take)
                .ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}