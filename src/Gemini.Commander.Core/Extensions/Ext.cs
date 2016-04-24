using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using DocoptNet;

namespace Gemini.Commander.Core.Extensions
{
    public static class Ext
    {
        public static double Round(this double value, int decimals = 1) => Math.Round(value, 1);
        public static T Extract<T>(this IDictionary<string, ValueObject> args, string key, T defaultValue) => args.ContainsKey(key) && args[key]?.Value != null ? (T)Convert.ChangeType(args[key]?.Value?.ToString(), typeof(T)) : defaultValue;
        public static int ConvertTo(this string hours, string tag)
        {
            var pattern = $@"(?<c>\d+){tag}";
            if (!Regex.IsMatch(hours, pattern, RegexOptions.IgnoreCase)) return 0;
            var value = Regex.Match(hours, pattern, RegexOptions.IgnoreCase).Groups["c"]?.Value;
            return Convert.ToInt32(value ?? "0");
        }



        public static IList<IssueTimeTrackingDto> LogsByUser(this ServiceManager svc, UserDto user)
        {
            return svc.Item.GetFilteredItems(new IssuesFilter
            {
                IncludeClosed = true,
                TimeLoggedBy = user.Entity.Id + ""
            })
            .SelectMany(x => x.TimeEntries.Where(e => e.Entity.UserId == user.Entity.Id))
            .ToList();
        }
    }
}