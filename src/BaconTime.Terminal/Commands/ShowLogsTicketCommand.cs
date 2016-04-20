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
    [Command("show", "logs", "ticket")]
    public class ShowLogsTicketCommand : ServiceManagerCommand
    {
        private FluentCommandLineParser p;
        private int limit=100;

        public ShowLogsTicketCommand(ServiceManager svc) : base(svc) { }

        public override void Execute(MainArgs args)
        {
            var user = Svc.Item.WhoAmI();
            var times = Svc.Item.GetTimes(args.Options.Id);

            var table = new ConsoleTable("user", "date", "hours", "message");

            times
                .OrderByDescending(x => x.Entity.EntryDate)
                .Where(x => !args.ArgMy || x.Entity.UserId == user.Entity.Id)
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