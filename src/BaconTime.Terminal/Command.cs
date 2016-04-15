using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;

namespace BaconTime.Terminal
{
    interface ICommand
    {
        void Execute(string[] args);
    }

    abstract class BaseCommand : ICommand
    {
        public abstract void Execute(string[] args);

        protected bool ExitOnMissingParams(ICommandLineParserResult result)
        {
            Console.WriteLine(result.ErrorText);
            return result.HasErrors;
        }

    }

    class LogHoursCommand : BaseCommand
    {
        private FluentCommandLineParser<IssueTimeTracking> p;

        public LogHoursCommand(ServiceManager svc, Configuration conf, User user)
        {
            var now = DateTime.Now;
            p = new FluentCommandLineParser<IssueTimeTracking>();

            p.Setup(x => x.IssueId).As('t', "ticket").Required();
            p.Setup(x => x.Comment).As('c', "comment").Required();

            p.Setup(x => x.Hours).As('h', "hours").SetDefault(0);
            p.Setup(x => x.Minutes).As('m', "minutes").SetDefault(0);
            p.Setup(x => x.TimeTypeId).As("time-type").SetDefault(30);
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
        }

        public override void Execute(string[] args)
        {
            ExitOnMissingParams(p.Parse(args));
        }
    }
}