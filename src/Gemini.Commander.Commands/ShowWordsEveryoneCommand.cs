using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Countersoft.Gemini.Api;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using Iveonik.Stemmers;
using Newtonsoft.Json;

namespace Gemini.Commander.Commands
{
    [Command("show", "words", "everyone")]
    public class ShowWordsEveryoneCommand : ServiceManagerCommand
    {
        public ShowWordsEveryoneCommand(ServiceManager svc) : base(svc) { }

        public static string Trim(string e) => e.ToLower().Trim(',', ';', ':', '.', '-', '+', '\r', '\n', '?', '!');

        public static Func<string, string> Stem(bool stemmed) => e => stemmed ? new EnglishStemmer().Stem(e) : e;

        public static bool Allowed(string e)
        {
            var stopwords = (ConfigurationManager.AppSettings["stopwords"] ?? "").Split(' ');
            return !stopwords.Contains(e) && !string.IsNullOrWhiteSpace(e) && e.Length > 1;
        }

        public static string Clean(string e) => Regex.Replace(e, @"\s+", " ");

        public override void Execute(MainArgs args)
        {
            var take = args.Options.Take;
            var items = Svc.LogsByEveryone(args);

            var words = items.GroupBy(x => x.Fullname).Select(x => new
            {
                user = x.Key,
                words = Dump(args, x).ToDictionary(d => d.Word, d => new { d.Percent, d.Count })
            }).ToDictionary(x => x.user, x => x.words);

            foreach (var word in words)
            {
                var labels = word.Value.OrderByDescending(x => x.Value.Percent).Take(6);
                var data = new
                {
                    labels = labels.Select(x => x.Key),
                    datasets = new
                    {
                        label = word.Key,
                        data = labels.Select(x => x.Value.Percent.Round())
                    }
                };
                Console.WriteLine(JsonConvert.SerializeObject(data));
            }
        }

        private static System.Collections.Generic.List<Item> Dump(MainArgs args, IGrouping<string, Countersoft.Gemini.Commons.Dto.IssueTimeTrackingDto> x)
        {
            return x.AllWords()
                    .Select(Clean)
                    .Select(Trim)
                    .Select(Stem(args.Options.Stemmed))
                    .Where(Allowed)
                    .GroupBy(m => m)
                    .OrderByDescending(m => m.Count())
                    .Select(m => new Item { Word = m.Key, Percent = (m.Count() * 100m / x.Count()), Count = m.Count() })
                    .ToList();
        }
    }

    public class Item
    {
        public string Word { get; set; }
        public decimal Percent { get; set; }
        public int Count { get; set; }
    }
}