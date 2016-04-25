using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using FluentAssertions;
using Gemini.Commander.Core;
using TechTalk.SpecFlow;

namespace Gemini.Commander.Spec.Steps
{
    [Binding]
    public sealed class TimeReportStepDefinition
    {
        [BeforeScenario()]
        public static void Before()
        {
            var s = ConfigurationManager.AppSettings;
            var svc = new ServiceManager(s["endpoint"], s["username"], "", s["apikey"]);
            var whoAmI = svc.Item.WhoAmI();
            var filteredItems = svc.Item.GetFilteredItems(new IssuesFilter { TimeLoggedBy = whoAmI.Entity.Id + "" });
            filteredItems.SelectMany(x => x.TimeEntries.Select(m => m.Entity)).ToList().ForEach(x => svc.Item.DeleteTime(x.IssueId, x.Id));
            ScenarioContext.Current.Set(svc);
        }

        [AfterScenario()]
        public static void After()
        {
            var svc = ScenarioContext.Current.Get<ServiceManager>();
            var ticket = ScenarioContext.Current.Get<IssueDto>();
            svc.Item.Delete(ticket.Id);
        }

        [Given(@"I have a ticket (.*)")]
        public void GivenIhaveATicket(string title)
        {
            var svc = ScenarioContext.Current.Get<ServiceManager>();
            var project = svc.Projects.GetProjects().First();
            var user = svc.Item.WhoAmI();

            var ticket = svc.Item.Create(new Issue
            {
                Title = title,
                Creator = user.Entity.Id,
                Active = true,
                ProjectId = project.Entity.Id
            });

            ScenarioContext.Current.Set(ticket);
        }

        [Given(@"^I execute log (.*)$")]
        [When(@"^I execute log (.*)$")]
        public void WhenIExecuteLogCommand(string command)
        {
            var ticket = ScenarioContext.Current.Get<IssueDto>();
            command = command.Replace("id", $" {ticket.Id} ");

            var report = StringExt.ConsoleSpy(() => new CommandRunner().Run(new MainArgs(command.ToArgs())));
            report.Trim().ShouldBeEquivalentTo("Time was logged.");
        }

        [Then(@"(.*), (.*) and the (.*) is added.")]
        public void ThenIsAndTheCommentIsAdded_(int total_minutes, string date, string comment)
        {
            var svc = ScenarioContext.Current.Get<ServiceManager>();
            var ticket = ScenarioContext.Current.Get<IssueDto>();

            var entryDate = (date == "today" ? DateTime.Today : Convert.ToDateTime(date)).ToUniversalTime().Date;

            svc.Item.GetTimes(ticket.Id).Any().Should().BeTrue();
            var entry = svc.Item.GetTimes(ticket.Id).First().Entity;
            (entry.Minutes + (entry.Hours * 60)).ShouldBeEquivalentTo(total_minutes);
            entry.Comment.ShouldBeEquivalentTo(comment);
            entry.EntryDate.ToUniversalTime().Date.ShouldBeEquivalentTo(entryDate);
        }

        [When(@"I execute show (.*)")]
        public void WhenIExecuteShowLog_TId(string command)
        {
            var ticket = ScenarioContext.Current.Get<IssueDto>();
            command = command.Replace("id", $" {ticket.Id} ");

            Console.WriteLine(command);
            var report = StringExt.ConsoleSpy(() => new CommandRunner().Run(new MainArgs(command.ToArgs())));
            ScenarioContext.Current.Set(report, "report");
        }

        [Then(@"message is shown")]
        public void ThenMessageIsShown(string expected)
        {
            var report = ScenarioContext.Current.Get<string>("report");
            Console.WriteLine(report);
            report.Should().Contain(expected);
        }
    }

    public static class StringExt
    {
        public static string[] ToArgs(this string entry)
        {
            return Regex
                  .Matches(entry, @"(?<match>[^\ ""]+)|\""(?<match>[^""]*)""")
                  .Cast<Match>()
                  .Select(m => m.Groups["match"].Value)
                  .ToArray();
        }

        public static string ConsoleSpy(Action executeCommand)
        {
            var originalConsoleOut = Console.Out;
            try
            {
                using (var writer = new StringWriter())
                {
                    Console.SetOut(writer);
                    executeCommand();
                    writer.Flush();

                    return writer.GetStringBuilder().ToString();
                }
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }
        }
    }
}
