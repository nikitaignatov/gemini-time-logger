using System.Collections.Generic;
using System.Linq;
using Countersoft.Gemini.Api;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using static Gemini.Commander.Core.Extensions.Ext;

namespace Gemini.Commander.Commands
{
    public class ShowWordsEveryoneQuery : ServiceManagerQuery<IEnumerable<Data>>
    {
        public ShowWordsEveryoneQuery(ServiceManager svc) : base(svc) { }

        public override IEnumerable<Data> Execute(MainArgs args)
        {
            var take = args.Options.Take;
            var items = Svc.LogsByEveryone(args);

            var words = items.GroupBy(x => x.Fullname).Select(x => new
            {
                user = x.Key,
                words = Dump(args, x).ToDictionary(d => d.Word, d => new { d.Percent, d.Count })
            }).ToDictionary(x => x.user, x => x.words);

            return words
                .Select(word => new { word, labels = word.Value.OrderByDescending(x => x.Value.Percent).Take(take) })
                .Select(item => new Data
                {
                    Labels = item.labels.Select(x => x.Key),
                    Datasets = new[]
                    {
                        new Dataset
                        {
                            Label = item.word.Key,
                            Data = item.labels.Select(x => x.Value.Percent.Round())
                        }
                    }
                });
        }

        private static IEnumerable<Item> Dump(MainArgs args, IGrouping<string, Countersoft.Gemini.Commons.Dto.IssueTimeTrackingDto> x)
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

    public class Dataset
    {
        public string Label { get; set; }
        public IEnumerable<decimal> Data { get; set; }
    }

    public class Data
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<Dataset> Datasets { get; set; }
    }
}