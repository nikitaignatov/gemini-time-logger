using System.Linq;
using BaconTime.Terminal.Extensions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    [Command("show", "hours", "my")]
    public class ShowLoggedHoursCommand : ServiceManagerCommand
    {
        public ShowLoggedHoursCommand(ServiceManager svc) : base(svc)
        {
        }

        public override void Execute(MainArgs args)
        {
            var user = Svc.Item.WhoAmI();

            var workingHours = args.Options.WorkingHours;
            var take = args.Options.Take;
            var items = Svc.Item
                .GetFilteredItems(new IssuesFilter
                {
                    IncludeClosed = true,
                    TimeLoggedBy = user.Entity.Id + "",
                    TimeLoggedAfter = args.Options.From.ToString("yyyy-MM-dd"),
                    TimeLoggedBefore = args.Options.To.ToString("yyyy-MM-dd")
                })
                .SelectMany(x => x.TimeEntries.Select(e => new { x.Entity, Time = e }))
                .ToList();

            var times = items.OrderByDescending(x => x.Time.Entity.EntryDate);

            var table = new ConsoleTable("date", "hours", "missing hours");

            times
                .Where(x => x.Time.Entity.UserId == user.Entity.Id)
                .Where(x => x.Time.Entity.EntryDate >= args.Options.From)
                .Where(x => x.Time.Entity.EntryDate <= args.Options.To)
                .GroupBy(x => x.Time.Entity.EntryDate.Date)
                .OrderByDescending(x => x.Key)
                .Select(x => new object[]
                {
                    x.Key.ToString("yyyy-MM-dd"),
                    x.Sum(m=>m.Time.Hours()),
                    workingHours- x.Sum(m=>m.Time.Hours())
                })
                .Take(take).ToList()
                .ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}