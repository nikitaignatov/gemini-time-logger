using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using DocoptNet;
using Iveonik.Stemmers;

namespace Gemini.Commander.Core.Extensions
{
    public static class Ext
    {
        public static decimal Round(this decimal value, int decimals = 1) => Math.Round(value, 1);
        public static double Round(this double value, int decimals = 1) => Math.Round(value, 1);
        public static T Extract<T>(this IDictionary<string, ValueObject> args, string key, T defaultValue) => args.ContainsKey(key) && args[key]?.Value != null ? (T)Convert.ChangeType(args[key]?.Value?.ToString(), typeof(T)) : defaultValue;
        public static int ConvertTo(this string hours, string tag)
        {
            var pattern = $@"(?<c>\d+){tag}";
            if (!Regex.IsMatch(hours, pattern, RegexOptions.IgnoreCase)) return 0;
            var value = Regex.Match(hours, pattern, RegexOptions.IgnoreCase).Groups["c"]?.Value;
            return Convert.ToInt32(value ?? "0");
        }

        public static T Id<T>(T x) => x;

        public static IList<IssueTimeTrackingDto> LogsByUser(this ServiceManager svc, UserDto user, MainArgs args)
        {
            return svc.Item.GetFilteredItems(new IssuesFilter
            {
                IncludeClosed = true,
                TimeLoggedBy = user.Entity.Id + "",
                TimeLoggedAfter = args.Options.From.ToString("yyyy-MM-dd"),
                TimeLoggedBefore = args.Options.To.ToString("yyyy-MM-dd")
            })
            .SelectMany(x => x.TimeEntries.Where(e => e.Entity.UserId == user.Entity.Id))
            .Where(x => x.Entity.EntryDate >= args.Options.From)
            .Where(x => x.Entity.EntryDate <= args.Options.To)
            .ToList();
        }

        public static IList<IssueTimeTrackingDto> LogsByEveryone(this ServiceManager svc, MainArgs args)
        {
            return svc.Item.GetFilteredItems(new IssuesFilter
            {
                IncludeClosed = true,
                TimeLoggedAfter = args.Options.From.ToString("yyyy-MM-dd"),
                TimeLoggedBefore = args.Options.To.ToString("yyyy-MM-dd")
            })
            .SelectMany(x => x.TimeEntries)
            .Where(x => x.Entity.EntryDate >= args.Options.From)
            .Where(x => x.Entity.EntryDate <= args.Options.To)
            .ToList();
        }


        public static string Trim(string e) => e.ToLower().Trim(',', ';', ':', '.', '-', '+', '\r', '\n', '?', '!');

        public static Func<string, string> Stem(bool stemmed) => e => stemmed ? new EnglishStemmer().Stem(e) : e;

        public static bool Allowed(string e)
        {
            var stopwords = (ConfigurationManager.AppSettings["stopwords"] ?? "").Split(' ');
            return !Enumerable.Contains(stopwords, e) && !String.IsNullOrWhiteSpace(e) && e.Length > 1;
        }

        public static string Clean(string e) => Regex.Replace(e, @"\s+", " ");
        public static IEnumerable<string> Normalized(this IEnumerable<string> e) => e.Select(Clean).Select(Trim).Select(Stem(false)).Where(Allowed);

        public static bool Between(this decimal value, decimal min, decimal max) => (value >= min && value <= max);

    }
}