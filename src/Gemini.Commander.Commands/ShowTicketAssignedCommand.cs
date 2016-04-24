using System.Linq;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Gemini.Commander.Core;
using Format = ConsoleTables.Core.Format;

namespace Gemini.Commander.Commands
{
    [Command("show", "ticket", "assigned")]
    public class ShowTicketAssignedCommand : ServiceManagerCommand
    {
        public ShowTicketAssignedCommand(ServiceManager svc) : base(svc) { }

        public override void Execute(MainArgs args)
        {
            var user = Svc.Item.WhoAmI();
            var projects = Svc.Item.GetFilteredItems(new IssuesFilter
            {
                Resources = "" + user.Entity.Id
            });
            var take = args.Options.Take;

            var table = new ConsoleTable("id", "ticket");

            projects
                .Select(x => new object[]
                {
                    x.Entity.Id,
                    x.Entity.Title
                })
                .Take(take).ToList().
                ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}