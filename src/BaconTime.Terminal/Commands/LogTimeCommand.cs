using System;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;

namespace BaconTime.Terminal.Commands
{
    [Command("log")]
    public class LogTimeCommand : ServiceManagerCommand
    {
        public LogTimeCommand(ServiceManager svc) : base(svc) { }
        public IssueTimeTracking ToIssueTimeTracking(MainArgs args)
        {
            return new IssueTimeTracking
            {
                IssueId = args.Options.Id,
                TimeTypeId = args.Options.LogType,
                Hours = args.Options.Hours,
                Minutes = args.Options.Minutes,
                Comment = args.OptMessage,
                EntryDate = args.OptWhen
            };
        }

        public override void Execute(MainArgs args)
        {
            var now = DateTime.Now;
            var log = ToIssueTimeTracking(args);
            if (log.Minutes + log.Hours == 0) throw new Exception("total time of 0, this is not possibleto log.");

            var user = Svc.Item.WhoAmI();
            var issue = Svc.Item.Get(log.IssueId);

            log.UserId = user.Entity.Id;
            log.Active = true;
            log.Archived = false;
            log.Deleted = false;
            log.Created = now;
            log.ProjectId = issue.Project.Id;

            Svc.Item.LogTime(log);
            Console.WriteLine("Time was logged.");
        }
    }
}