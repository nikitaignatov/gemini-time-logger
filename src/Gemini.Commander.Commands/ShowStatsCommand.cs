using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;
using Format = ConsoleTables.Core.Format;

namespace Gemini.Commander.Commands
{
    [Command("show", "stats")]
    public class ShowStatsCommand : ServiceManagerCommand
    {
        public ShowStatsCommand(ServiceManager svc) : base(svc) { }

        public override void Execute(MainArgs args)
        {
            var take = args.Options.Take;
            var user = Svc.Item.WhoAmI();
            var items = Svc.LogsByUser(user, args);
          
            if (!items.Any())
            {
                Console.WriteLine("no entries during the period.");
                return;
            }
            var groupByEntryDate = items.GroupByEntryDate().ToList();
            var dict = new Dictionary<string, IEnumerable<double>>
            {
                {"entry_creation_spread", items.Select(x => (x.Entity.Created - x.Entity.EntryDate).TotalDays).ToList()},
                {"hours_pr_entry       ", items.Select(x =>x.Hours()).ToDouble()},
                {"hours_pr_day         ", groupByEntryDate.Select(x => x.Hours()).ToDouble()},
                {"words_pr_entry       ", items.Select(x =>x.Words().Count()).ToDouble()},
                {"words_pr_day         ", groupByEntryDate.Select(x => x.AllWords().Count()).ToDouble()},
                {"entries_pr_day       ", groupByEntryDate.Select(x => x.Count()).ToDouble()},
                {"projects_pr_day      ", groupByEntryDate.Select(x => x.Select(e=>e.ProjectId).Distinct().Count()).ToDouble()},
                {"tickets_pr_day       ", groupByEntryDate.Select(x => x.Select(e=>e.IssueId).Distinct().Count()).ToDouble()},
                {"time_logging_day     ", items.GroupBy(x=>x.Entity.Created.Date.Day).OrderByDescending(x=>x.Count()).Select(x => x.Key).ToDouble()},
                {"time_logging_time    ", items.GroupBy(x=>x.Entity.Created.Hour).OrderByDescending(x=>x.Count()).Select(x => x.Key).ToDouble()},
                {"overtime_pr_day      ", groupByEntryDate.Select(x =>Math.Max(0, x.Hours()-8)).ToDouble()},
                {"abscence_pr_day      ", groupByEntryDate.Select(x =>Math.Abs(Math.Min(0, x.Hours()-8))).ToDouble()},
            };

            var table = new ConsoleTable("stat", "size", "sum", "min", "median", "mean", "lower q", "upper q", "90th q", "99th q", "max");

            dict
                .Select(x => new object[]
                {
                    x.Key,
                    x.Value.Count(),
                    x.Value.Sum().Round(),
                    x.Value.Min().Round(),
                    x.Value.Median().Round(),
                    x.Value.Mean().Round(),
                    x.Value.LowerQuartile().Round(),
                    x.Value.UpperQuartile().Round(),
                    x.Value.Percentile(90).Round(),
                    x.Value.Percentile(99).Round(),
                    x.Value.Max().Round(),
                })
                .Take(take)
                .ToList()
                .ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}