using System;
using System.Collections.Generic;
using System.Linq;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;

namespace BaconTime.Terminal.Extensions
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

        public static string Shorten(this string input, int length) => string.Join("", input.Take(length));
    }
}
