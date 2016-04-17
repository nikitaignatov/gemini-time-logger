using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public TimeLoggedForItemCommand(ServiceManager svc)
        {
            this.svc = svc;
            p = new FluentCommandLineParser();

            p.Setup<int>('t', "ticket").Required().Callback(x => issueId = x).WithDescription("Id of the ticket, for which the log entries shouldbe loaded.");
            p.Setup<int>('l', "limit").SetDefault(10).Callback(x => limit = x).WithDescription("Limit number of entries to be returned");

            p.SetupHelp("?", "help").Callback(x => Console.WriteLine(x));
        }

        public override void Execute(string[] args)
        {
            ValidateParams(p.Parse(args));

            var issue = svc.Item.Get(issueId);
            var times = svc.Item.GetTimes(issueId);

            Console.WriteLine($"\n\n-> {issue.Title}:\n");

            var table = new ConsoleTable("date", "hors", "message");

            times
                .OrderByDescending(x => x.Entity.EntryDate)
                .Take(limit)
                .Select(x => new { date = x.Entity.EntryDate.ToString("yyyy-MM-dd"), Hours = Math.Round(x.Minutes() / 60m, 1), Message = string.Join("", x.Entity.Comment.Take(25)) })
                .ForEach(x => table.AddRow(x.date, x.Hours, x.Message));
            table.Write(Format.MarkDown);
        }
    }
}