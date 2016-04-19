using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using DocoptNet;
using Fclp;

namespace BaconTime.Terminal.Commands
{
    public class LogHoursCommand
    {
        private readonly ServiceManager svc;
        public LogHoursCommand(ServiceManager svc)
        {
            this.svc = svc;
        }

        public IssueTimeTracking ToIssueTimeTracking(MainArgs args)
        {
            return new IssueTimeTracking
            {
                IssueId = args.OptTicket,
                TimeTypeId = args.OptTimeType,
                Hours = args.OptHours,
                Minutes = args.OptMinutes,
                Comment = args.OptMessage,
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