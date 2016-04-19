using System;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;

namespace BaconTime.Terminal.Commands
{
    public class LogTimeCommand
    {
        private readonly ServiceManager svc;
        public LogTimeCommand(ServiceManager svc) { this.svc = svc; }

        public IssueTimeTracking ToIssueTimeTracking(MainArgs args)
        {
            return new IssueTimeTracking
            {
                IssueId = args.OptTicket,
                TimeTypeId = args.OptLogType,
                Hours = args.OptHours,
                Minutes = args.OptMinutes,
                Comment = args.OptMessage,
                EntryDate = args.OptWhen
            };
        }

        public void Execute(MainArgs args)
        {
            var now = DateTime.Now;
            var log = ToIssueTimeTracking(args);
            if (log.Minutes + log.Hours == 0) throw new Exception("total time of 0, this is not possibleto log.");

            var user = svc.Item.WhoAmI();
            var issue = svc.Item.Get(log.IssueId);

            log.UserId = user.Entity.Id;
            log.Active = true;
            log.Archived = false;
            log.Deleted = false;
            log.Created = now;
            log.ProjectId = issue.Project.Id;

            svc.Item.LogTime(log);
            Console.WriteLine("Time was logged.");
        }
    }
}