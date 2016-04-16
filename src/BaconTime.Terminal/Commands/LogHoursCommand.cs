using System;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;

namespace BaconTime.Terminal.Commands
{
    public class LogHoursCommand : BaseCommand
    {
        private readonly ServiceManager svc;
        private readonly User user;
        private FluentCommandLineParser<IssueTimeTracking> p;

        public LogHoursCommand(ServiceManager svc, User user)
        {
            this.svc = svc;
            this.user = user;
            p = new FluentCommandLineParser<IssueTimeTracking>();

            p.Setup(x => x.IssueId).As('t', "ticket").Required().WithDescription("the id of te issue number.");
            p.Setup(x => x.Comment).As('c', "comment").Required();

            p.Setup(x => x.Hours).As('h', "hours").SetDefault(0);
            p.Setup(x => x.Minutes).As('m', "minutes").SetDefault(0);
            p.Setup(x => x.TimeTypeId).As("time-type").SetDefault(30);
        }

        public override void Execute(string[] args)
        {
            ValidateParams(p.Parse(args));

            var now = DateTime.Now;
            var log = p.Object;

            if (log.Minutes + log.Hours == 0) throw new Exception("-h and -m for hours and minutes are missing.");

            var issue = svc.Item.Get(log.IssueId);

            log.UserId = user.Id;
            log.EntryDate = now;
            log.Active = true;
            log.Archived = false;
            log.Deleted = false;
            log.Created = now;
            log.TimeTypeId = 30;
            log.ProjectId = issue.Project.Id;

            svc.Item.LogTime(log);
            Console.WriteLine("Time was logged.");
        }
    }
}