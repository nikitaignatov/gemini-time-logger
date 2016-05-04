using System;
using System.Collections.Generic;
using System.Linq;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;

namespace Gemini.Commander.Core.Extensions
{
    public static class IssueTimeTrackingExtensions
    {
        public static decimal Hours(this IssueTimeTracking time) => Math.Round(time.Minutes() / 60m, 1);
        public static decimal Hours(this IssueTimeTrackingDto time) => time.Entity.Hours();
        public static decimal Hours(this IEnumerable<IssueTimeTrackingDto> times) => times.Select(Hours).Sum();
        public static decimal Hours(this IEnumerable<IssueTimeTracking> times) => times.Select(Hours).Sum();

        public static int Minutes(this IssueTimeTracking time) => (time.Minutes + (time.Hours * 60));
        public static int Minutes(this IssueTimeTrackingDto time) => time.Entity.Minutes();
        public static int Minutes(this IEnumerable<IssueTimeTrackingDto> times) => times.Select(Minutes).Sum();
        public static int Minutes(this IEnumerable<IssueTimeTracking> times) => times.Select(Minutes).Sum();

        public static IEnumerable<string> Words(this IssueTimeTracking time) => time.Comment.Split(' ');
        public static IEnumerable<string> Words(this IssueTimeTrackingDto time) => time.Entity.Words();
        public static IEnumerable<string> AllWords(this IEnumerable<IssueTimeTrackingDto> times) => times.SelectMany(Words);
        public static IEnumerable<string> AllWords(this IEnumerable<IssueTimeTracking> times) => times.SelectMany(Words);
        
        public static string Shorten(this string input, int length) => string.Join("", input.Take(length));

        public static IEnumerable<IGrouping<DateTime, IssueTimeTracking>> GroupByEntryDate(this IEnumerable<IssueTimeTrackingDto> times) => times.Select(x => x.Entity).GroupByEntryDate();
        public static IEnumerable<IGrouping<DateTime, IssueTimeTracking>> GroupByEntryDate(this IEnumerable<IssueTimeTracking> times) => times.GroupBy(x => x.EntryDate.Date);
        public static IEnumerable<double> ToDouble(this IEnumerable<int> times) => times.Select(Convert.ToDouble);
        public static IEnumerable<double> ToDouble(this IEnumerable<float> times) => times.Select(Convert.ToDouble);
        public static IEnumerable<double> ToDouble(this IEnumerable<decimal> times) => times.Select(Convert.ToDouble);
    }
}
