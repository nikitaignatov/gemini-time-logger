using System;
using System.Linq;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using Fclp;

namespace BaconTime.Terminal
{
    class Program
    {
        private static ServiceManager svc;
        private static Configuration config;
        private static User user;
        private static IssueTimeTracking log;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var now = DateTime.Now;

            var p = new FluentCommandLineParser<IssueTimeTracking>();
            var c = new FluentCommandLineParser<Configuration>();

            if (LoadService(args, c)) return;
            LoadUser();

            p.Setup(x => x.IssueId).As('t', "ticket").Required();
            p.Setup(x => x.Comment).As('c', "comment").Required();

            p.Setup(x => x.Hours).As('h', "hours").SetDefault(0);
            p.Setup(x => x.Minutes).As('m', "minutes").SetDefault(0);
            p.Setup(x => x.TimeTypeId).As("time-type").SetDefault(30);
            log = p.Object;

            if (ExitOnMissingParams(p.Parse(args))) return;

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

            var times = svc.Item.GetTimes(log.IssueId);

            Console.WriteLine($"Ticket: {issue.Title} \nRecent entries: \n");
            Console.WriteLine(string.Join("\n", times.OrderByDescending(x => x.Entity.EntryDate).Take(5).Select(x => $"{x.Entity.Created}: {x.Fullname}: {x.Entity.Hours}h {x.Entity.Minutes}m message: {string.Join("", x.Entity.Comment.Take(25))}...")));
        }

        private static bool LoadService(string[] args, FluentCommandLineParser<Configuration> c)
        {
            c.Setup(x => x.endpoint).As('e', "endpoint").Required();
            c.Setup(x => x.username).As('u', "username").Required();
            c.Setup(x => x.key).As('k', "apikey").Required();

            if (ExitOnMissingParams(c.Parse(args))) return true;
            config = c.Object;
            svc = new ServiceManager(config.endpoint, config.username, "", config.key);
            return false;
        }

        private static bool ExitOnMissingParams(ICommandLineParserResult result)
        {
            Console.WriteLine(result.ErrorText);
            return result.HasErrors;
        }

        private static void LoadUser()
        {
            try
            {
                user = svc.User.GetUsers().First(x => x.Entity.Username == config.username).Entity;
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not find user with username: {config.username}");
                throw;
            }
        }
    }
}
