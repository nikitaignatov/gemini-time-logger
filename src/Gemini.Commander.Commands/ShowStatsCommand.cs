using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using MathNet.Numerics.Statistics;
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

            var dict = new Dictionary<string, IEnumerable<double>>
            {
                {"entry_creation_spread     (days)", items.Select(x => (x.Entity.Created - x.Entity.EntryDate).TotalDays).ToList()},
                {"hours_pr_day              (hours)", items.GroupByEntryDate().Select(x => x.Hours()).ToDouble()},
                {"entries_pr_day            (hours)", items.GroupByEntryDate().Select(x => x.Count()).ToDouble()},
                {"projects_pr_day           (hours)", items.GroupByEntryDate().Select(x => x.Select(e=>e.ProjectId).Distinct().Count()).ToDouble()},
                {"time_logging_day          (day of month)", items.GroupBy(x=>x.Entity.Created.Date.Day).OrderByDescending(x=>x.Count()).Select(x => x.Key).ToDouble()},
                {"time_logging_time         (hour of day)", items.GroupBy(x=>x.Entity.Created.Hour).OrderByDescending(x=>x.Count()).Select(x => x.Key).ToDouble()},
                {"overtime_by               (hours)", items.GroupByEntryDate().Select(x =>Math.Max(0, x.Hours()-8)).ToDouble()},
                {"short_days_by             (hours)", items.GroupByEntryDate().Select(x =>Math.Abs(Math.Min(0, x.Hours()-8))).ToDouble()},
                {"hours_pr_entry            (hours)", items.Select(x =>x.Hours()).ToDouble()},
            };

            var table = new ConsoleTable("stat", "min", "max", "median", "mean", "lower q", "upper q", "90th q");

            dict
                .Select(x => new object[]
                {
                    x.Key,
                    x.Value.Min().Round(),
                    x.Value.Max().Round(),
                    x.Value.Median().Round(),
                    x.Value.Mean().Round(),
                    x.Value.LowerQuartile().Round(),
                    x.Value.UpperQuartile().Round(),
                    x.Value.Percentile(90).Round(),
                })
                .Take(take)
                .ToList()
                .ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }
    }
}