using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using Iveonik.Stemmers;
using Newtonsoft.Json;
using Format = ConsoleTables.Core.Format;

namespace Gemini.Commander.Commands
{
    [Command("show", "words", "all")]
    public class ShowWordsCommand : ServiceManagerCommand
    {
        public ShowWordsCommand(ServiceManager svc) : base(svc) { }

        public static string Trim(string e) => e.ToLower().Trim(',', ';', ':', '.', '-', '+', '\r', '\n');

        public static Func<string, string> Stem(bool stemmed) => e => stemmed ? new EnglishStemmer().Stem(e) : e;

        public static bool Allowed(string e)
        {
            var stopwords = (ConfigurationManager.AppSettings["stopwords"] ?? "").Split(' ');
            return !stopwords.Contains(e) && !string.IsNullOrWhiteSpace(e) && e.Length > 1;
        }

        public static string Clean(string e) => Regex.Replace(e.ToLower(), @"\s+", " ");

        public override void Execute(MainArgs args)
        {
            var take = args.Options.Take;
            var user = Svc.Item.WhoAmI();
            var items = Svc.LogsByUser(user, args);
            
            var words = items.SelectMany(e => EntryToWords(args, e))
                .GroupBy(m => m)
                .OrderByDescending(m => m.Count())
                .Select(m => new { m.Key, pct = (m.Count() * 100m / items.Count()) })
                .ToList();

            var table = new ConsoleTable("word", "percent");

            words
                .Select(x => new object[]
                {
                    x.Key,x.pct.ToString("F1")
                })
                .Take(take)
                .ToList()
                .ForEach(x => table.AddRow(x));

            table.Write(Format.MarkDown);
        }

        public static IEnumerable<string> EntryToWords(MainArgs args, IssueTimeTrackingDto e)
        {
            return e.Words()
                .Select(Clean)
                .Select(Trim)
                .Select(Stem(args.Options.Stemmed))
                .Where(Allowed)
                .Distinct();
        }
    }
}