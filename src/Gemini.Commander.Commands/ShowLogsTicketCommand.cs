using System.Linq;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using Format = ConsoleTables.Core.Format;

namespace Gemini.Commander.Commands
{
    [Command("show", "logs", "ticket")]
    public class ShowLogsTicketCommand : ServiceManagerCommand
    {
        public ShowLogsTicketCommand(ServiceManager svc) : base(svc) { }

        public override void Execute(MainArgs args)
        {
            var user = Svc.Item.WhoAmI();
            var times = Svc.Item.GetTimes(args.Arguments.Id);
            var take = args.Options.Take;

            var table = new ConsoleTable("user", "date", "hours", "message");

            times
                .OrderByDescending(x => x.Entity.EntryDate)
                .Where(x => x.Entity.UserId == user.Entity.Id)
                .Take(take)
                .Select(x => new
                {
                    user = x.Fullname,
                    date = x.Entity.EntryDate.ToString("yyyy-MM-dd"),
                    Hours = x.Hours(),
                    Message = string.Join("", x.Entity.Comment.Take(25))
                })
                .ToList()
                .ForEach(x => table.AddRow(x.user, x.date, x.Hours, x.Message));

            table.Write(Format.MarkDown);
        }
    }
}