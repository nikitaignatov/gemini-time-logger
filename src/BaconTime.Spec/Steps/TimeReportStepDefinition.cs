using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BaconTime.Terminal.Commands;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using FluentAssertions;
using NSubstitute;
using TechTalk.SpecFlow;

namespace BaconTime.Spec.Steps
{
    [Binding]
    public sealed class TimeReportStepDefinition
    {
        [BeforeScenario()]
        public static void Before()
        {
            var s = ConfigurationManager.AppSettings;
            var svc = new ServiceManager(s["endpoint"], s["username"], "", s["key"]);
            var project = svc.Projects.GetProjects().First();
            var user = svc.Item.WhoAmI();

            var ticket = svc.Item.Create(new Issue
            {
                Title = "Silly ticket",
                Creator = user.Entity.Id,
                Active = true,
                ProjectId = project.Entity.Id
            });

            ScenarioContext.Current.Set(ticket);
            ScenarioContext.Current.Set(svc);
        }
        [AfterScenario()]
        public static void After()
        {
            var svc = ScenarioContext.Current.Get<ServiceManager>();
            var ticket = ScenarioContext.Current.Get<IssueDto>();
            svc.Item.Delete(ticket.Id);
        }

        [Given(@"I have a ticket")]
        public void GivenIhaveATicket()
        {
            ScenarioContext.Current.Get<IssueDto>().Should().NotBeNull();
        }

        [When(@"^I log (.*)$")]
        public void WhenILogAndHiMom(string entry)
        {
            var svc = ScenarioContext.Current.Get<ServiceManager>();
            var ticket = ScenarioContext.Current.Get<IssueDto>();
            entry = entry.Replace("id", ticket.Id.ToString());

            new LogHoursCommand(svc).Execute(entry.ToArgs());
        }

        [Then(@"(.*) is and the (.*) is added.")]
        public void ThenIsAndTheCommentIsAdded_(int total_minutes, string comment)
        {
            var svc = ScenarioContext.Current.Get<ServiceManager>();
            var ticket = ScenarioContext.Current.Get<IssueDto>();

            svc.Item.GetTimes(ticket.Id).Any().Should().BeTrue();
            var entry = svc.Item.GetTimes(ticket.Id).First().Entity;
            (entry.Minutes + (entry.Hours * 60)).ShouldBeEquivalentTo(total_minutes);
            Console.WriteLine(entry.Comment);
            entry.Comment.ShouldBeEquivalentTo(comment);
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
    }
}
