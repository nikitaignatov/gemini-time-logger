using System;
using System.Linq;
using BaconTime.Terminal.Extensions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    public class TimeLoggedByUserGroupedByItemCommand : ServiceManagerCommand
    {
        public TimeLoggedByUserGroupedByItemCommand(ServiceManager svc) : base(svc)
        {
        }

        public override void Execute(MainArgs args)
        {
            var includeClosedTickets = args.Options.IncludeClosed;
            var my = args.ArgMy;
            var take = args.Options.Take;
            var user = Svc.Item.WhoAmI();
            var items = Svc.Item.GetFilteredItems(new IssuesFilter
            {
                IncludeClosed = includeClosedTickets,
                TimeLoggedBy = user.Entity.Id + ""
            }).SelectMany(x => x.TimeEntries.Select(e => new { x.Entity, Time = e })).ToList();

            var times = items.OrderByDescending(x => x.Time.Entity.EntryDate);

            var table = new ConsoleTable("user", "id", "ticket", "date", "hours", "message");

            times
                .Where(x => !my || x.Time.Entity.UserId == user.Entity.Id)
                .Select(x => new object[]
                {
                    x.Time.Fullname.Shorten(10),
                    x.Entity.Id,
                    x.Entity.Title.Shorten(20),
                    x.Time.Entity.EntryDate.ToString("yyyy-MM-dd"),
                    x.Time.Hours(),
                    x.Time.Entity.Comment.Shorten(25)
                })
                .Take(take)
                .ToList()
                .ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}