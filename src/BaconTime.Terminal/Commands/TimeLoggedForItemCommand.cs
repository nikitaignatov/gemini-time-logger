using System;
using System.Linq;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;

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

            p.Setup<int>('t', "ticket").Required().Callback(x => issueId = x);
            p.Setup<int>('l', "limit").SetDefault(10).Callback(x => limit = x);
        }

        public override void Execute(string[] args)
        {
            ValidateParams(p.Parse(args));

            var issue = svc.Item.Get(issueId);
            var times = svc.Item.GetTimes(issueId);

            Console.WriteLine($"Ticket: {issue.Title} \nRecent entries: \n");
            var items = times.OrderByDescending(x => x.Entity.EntryDate)
                            .Take(limit)
                            .Select(x => $"{x.Entity.Created}: {x.Fullname}: {x.Entity.Hours}h {x.Entity.Minutes}m message: {string.Join("", x.Entity.Comment.Take(25))}...");
            var report = string.Join("\n", items);

            Console.WriteLine(report);
        }
    }

    public class TimeLoggedByUserGroupedByItemCommand : BaseCommand
    {
        private readonly ServiceManager svc;
        private FluentCommandLineParser p;
        private int issueId;
        private int limit;

        public TimeLoggedByUserGroupedByItemCommand(ServiceManager svc)
        {
            this.svc = svc;
            p = new FluentCommandLineParser();

            p.Setup<int>('l', "limit").SetDefault(10).Callback(x => limit = x);
        }

        public override void Execute(string[] args)
        {
            ValidateParams(p.Parse(args));
            
            var user = svc.Item.WhoAmI();
            var times = svc.Item.GetFilteredItems(new IssuesFilter
            {
                IncludeClosed = true,
                TimeLoggedBy = user.Entity.Id + ""
            }).SelectMany(x => x.TimeEntries);

            //Console.WriteLine($"Ticket: {issue.Title} \nRecent entries: \n");
            var items = times.OrderByDescending(x => x.Entity.EntryDate)
                            .Take(limit)
                            .Select(x => $"{x.Entity.Created}: {x.Fullname}: {x.Entity.Hours}h {x.Entity.Minutes}m message: {string.Join("", x.Entity.Comment.Take(25))}...");
            var report = string.Join("\n", items);

            Console.WriteLine(report);
        }
    }
}